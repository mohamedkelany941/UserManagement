using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Users.BLL.Common;
using Users.BLL.Helpers;
using Users.BLL.Models;
using Users.BLL.Services.Interfaces;
using Users.DAL.Entities;

namespace Users.BLL.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;
        private readonly IIdentityService _IdentityService;

        private readonly IBaseRepository<UserManager<ApplicationUser>> repository = null;
        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt, IIdentityService identityService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
            _IdentityService = identityService;
        }

        public async Task<UserModel> RegisterAsync(RegisterModel model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return new UserModel { Message = "Email is already registered!" };

            if (await _userManager.FindByNameAsync(model.Username) is not null)
                return new UserModel { Message = "Username is already registered!" };

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                    errors += $"{error.Description},";

                return new UserModel { Message = errors };
            }

            await _userManager.AddToRoleAsync(user, "User");

            var jwtSecurityToken = await _IdentityService.CreateJwtToken(user);

            return new UserModel
            {
                Email = user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = user.UserName
            };
        }

        public async Task<Output<bool>> EditAsync(RegisterModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                }
                else
                {
                    user.UserName = model.Username;
                    user.Email = model.Email;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;

                    await _userManager.UpdateAsync(user);
                }
            return new Output<bool>(true);
            }
            catch (Exception ex)
            {
                return new Output<bool>(false);
                throw;
            }
           
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return false;
            }
            else
            {
                await _userManager.DeleteAsync(user);
                return true;
            }
        }

        public async Task<UserDataModel> GetByIdAsync(string id)
        {
            try
            {
                ApplicationUser appuser = await _userManager.FindByIdAsync(id);
                if (appuser == null)
                {
                    throw new NotFoundException("User", id);
                }
                else
                {
                    var user = new UserDataModel
                    {
                        UserName = appuser.UserName,
                        Email = appuser.Email,
                        FirstName = appuser.FirstName,
                        LastName = appuser.LastName
                    };

                    return user;
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
           
        }
        public async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.TokenLifetime),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        public async Task<UserModel> GetTokenAsync(TokenRequestModel model)
        {
            var UserModel = new UserModel();

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                UserModel.Message = "Email or Password is incorrect!";
                return UserModel;
            }

            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            UserModel.IsAuthenticated = true;
            UserModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            UserModel.Email = user.Email;
            UserModel.Username = user.UserName;
            UserModel.ExpiresOn = jwtSecurityToken.ValidTo;
            UserModel.Roles = rolesList.ToList();

            return UserModel;
        }

        public async Task<string> AddRoleAsync(AddRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user is null || !await _roleManager.RoleExistsAsync(model.Role))
                return "Invalid user ID or Role";

            if (await _userManager.IsInRoleAsync(user, model.Role))
                return "User already assigned to this role";

            var result = await _userManager.AddToRoleAsync(user, model.Role);

            return result.Succeeded ? string.Empty : "Something went wrong";
        }
    }
}
