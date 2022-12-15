using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.BLL.Common;
using Users.BLL.Models;
using Users.BLL.Services.Interfaces;
using Users.DAL.Entities;

namespace Users.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _UserService;

        public UserController(IUserService UserService)
        {
            _UserService = UserService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _UserService.RegisterAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<bool> DeleteAsync(string id)
        {
            return await _UserService.DeleteAsync(id);
        }

        [HttpPut("Edit")]
        public async Task<Output<bool>> EditAsync([FromBody] RegisterModel model)
        {
            return await _UserService.EditAsync(model);
        }

        [HttpGet("GetById")]
        public async Task<UserDataModel> GetByIdAsync(string id)
        {
            return await _UserService.GetByIdAsync(id);
        }

        [HttpPost("GetToken")]
        public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _UserService.GetTokenAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }


    }

}
