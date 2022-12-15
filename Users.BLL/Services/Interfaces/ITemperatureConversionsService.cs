using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.BLL.Common;
using Users.BLL.Models;
using Users.DAL.Entities;

namespace Users.BLL.Services.Interfaces
{
    public interface ITemperatureConversionsService
    {
        Output<FahrenheitPropertiesModel> ConvertFromCelsiusToFahrenheit(CelsiusPropertiesModel model);
     
    }
}
