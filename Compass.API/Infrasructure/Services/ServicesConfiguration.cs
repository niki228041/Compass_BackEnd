

using Compass.Data.Data.Classes;
using Compass.Data.Data.Interfaces;
using Compass.Services;
using Compass.Services.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Compass.API.Infrasructure.Services
{
    public class ServicesConfiguration
    {
        public static void Configuration(IServiceCollection services, IConfiguration _configuration)
        {
            
            // Add user service
            services.AddTransient<UserService>();
            // Add email service
            services.AddTransient<EmailService>();
            // Add JwtService
            services.AddTransient<JwtService>();
        }
    }
}
