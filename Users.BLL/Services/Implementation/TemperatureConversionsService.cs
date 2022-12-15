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
    public class TemperatureConversionsService : ITemperatureConversionsService
    {
        public Output<FahrenheitPropertiesModel> ConvertFromCelsiusToFahrenheit(CelsiusPropertiesModel model)
        {

            try
            {
                var _fahrenheitdegree = (model.CelsiusDegree * 9) / 5 + 32;
                var FahProp = new FahrenheitPropertiesModel { FahrenheitDegree = _fahrenheitdegree };
                return new Output<FahrenheitPropertiesModel>
                {
                    Value = FahProp
                };
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

    }
}
