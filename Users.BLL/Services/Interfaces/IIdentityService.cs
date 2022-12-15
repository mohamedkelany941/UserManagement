using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.BLL.Models;
using Users.DAL.Entities;

namespace Users.BLL.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user);
        Task<UserModel> GetTokenAsync(TokenRequestModel model);
        Task<string> AddRoleAsync(AddRoleModel model);
    }
}
