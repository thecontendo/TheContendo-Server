using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// using Vistex.Cloud.Services.Extensions.SwaggerService.Filters;

namespace Vistex.Cloud.Services.Extensions.SwaggerService
{
    public static class SwaggerServiceRegistrationExtenstions
    {
        public static void RegisterSwaggerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("api.public", new OpenApiInfo { Title = "Contendo Public Api", Version = "api.public" });
                c.SwaggerDoc("api.admin", new OpenApiInfo { Title = "Contendo Admin Api", Version = "api.admin" });
                c.AddSecurityDefinition("Public", GetSwaggerTokenSecurityScheme());
                c.AddSecurityDefinition("Admin", GetSwaggerTokenSecurityScheme());
                c.OperationFilter<AuthorizationOperationFilter>();
            });
        }
        
        private static OpenApiSecurityScheme GetSwaggerTokenSecurityScheme()
        {
            return new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header..."
            };
        }
    }
}
