using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.BLL.Services.Implementation;
using Users.BLL.Services.Interfaces;

namespace Users.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBLL(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<ITemperatureConversionsService, TemperatureConversionsService>();

            return services;
        }

    }
}
