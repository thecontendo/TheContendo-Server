using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Contendo.Extensions.AuthService
{
    public static class AuthServiceRegistrationExtensions
    {
        public static void RegisterAuthServices(this IServiceCollection services, IConfiguration configuration, ILogger _logger)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "api.public";
                options.DefaultChallengeScheme = "api.public";
            }).AddJwtBearer("api.public", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["InternalAuth:Issuer"],
                    ValidAudience = configuration["InternalAuth:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["InternalAuth:Key"])),
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = (context) =>
                    {
                        _logger.LogInformation($"RegisterAuthServices : Bearer Token api.ui, Token is validated");

                        return Task.CompletedTask;
                    }
                };
            });
        }
    }
}