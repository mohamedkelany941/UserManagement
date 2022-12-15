using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TemperatureConversionsController : ControllerBase
    {
        private readonly ITemperatureConversionsService _ITemperatureConversionsService;
        private readonly ILogger<TemperatureConversionsController> logger;
        public TemperatureConversionsController(ITemperatureConversionsService TemperatureConversionsService)
        {
            _ITemperatureConversionsService = TemperatureConversionsService;
        }

        [HttpPost("ConvertFromCelsiusToFahrenheit")]
        public ActionResult ConvertFromCelsiusToFahrenheit([FromBody] CelsiusPropertiesModel model)
        {
           
            try
            {
                return Ok(_ITemperatureConversionsService.ConvertFromCelsiusToFahrenheit(model));
            }
            catch(Exception ex)
            {
                //throw;// new logger.LogError(ex.Message);
                logger.LogError(ex.Message);
                return Ok();
            }

        }

    }

}
