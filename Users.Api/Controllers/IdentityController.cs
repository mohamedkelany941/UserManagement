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
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _IdentityService;

        public IdentityController(IIdentityService IdentityService)
        {
            _IdentityService = IdentityService;
        }

        [HttpPost("GetToken")]
        public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _IdentityService.GetTokenAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }
   
    }

}
