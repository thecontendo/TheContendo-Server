using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistex.Cloud.Services.Extensions.SwaggerService.Filters;

namespace Vistex.Cloud.Services.Extensions.SwaggerService
{
    public static class SwaggerServiceRegistrationExtenstions
    {
        public static void RegisterSwaggerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc("api.public", new OpenApiInfo { Title = "Public API", Version = "api.public" });
                c.SwaggerDoc("api.adminUi", new OpenApiInfo { Title = "Admin API", Version = "api.adminUi" });
                c.SwaggerDoc("api.ui", new OpenApiInfo { Title = "Tenant API", Version = "api.ui" });

                c.AddSecurityDefinition("Public", GetSwaggerClientSecurityScheme(false));
                c.AddSecurityDefinition("Admin", GetSwaggerClientSecurityScheme(true));
                c.AddSecurityDefinition("Tenant", GetSwaggerTokenSecurityScheme());

                c.OperationFilter<AuthorizationOperationFilter>();
            });
        }

        private static OpenApiSecurityScheme GetSwaggerClientSecurityScheme(bool isAdmin)
        {
            var path = isAdmin ? "GetAdminToken" : "GetPublicToken";
            return new OpenApiSecurityScheme
            {
                Description = "Oauth2",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows()
                {
                    ClientCredentials = new OpenApiOAuthFlow()
                    {
                        TokenUrl = new Uri($"/api/ui/InternalAuth/{path}", UriKind.Relative)
                    }
                }
            };
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
