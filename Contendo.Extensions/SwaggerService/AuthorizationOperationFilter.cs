using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Vistex.Cloud.Services.Extensions.SwaggerService
{
    public class AuthorizationOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Get Authorize attribute
            var attributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                                    .Union(context.MethodInfo.GetCustomAttributes(true))
                                    .OfType<AuthorizeAttribute>();

            if (attributes != null && attributes.Count() > 0)
            {
                var attr = attributes.ToList()[0];

                // Add what should be show inside the security section
                IList<string> securityInfos = new List<string>();
                securityInfos.Add($"{nameof(AuthorizeAttribute.Policy)}:{attr.Policy}");
                securityInfos.Add($"{nameof(AuthorizeAttribute.Roles)}:{attr.Roles}");
                securityInfos.Add($"{nameof(AuthorizeAttribute.AuthenticationSchemes)}:{attr.AuthenticationSchemes}");

                switch (attr.AuthenticationSchemes)
                {
                    case "api.public":
                        operation.Security = new List<OpenApiSecurityRequirement>() {
                                   new OpenApiSecurityRequirement
                                    {
                                        {
                                             new OpenApiSecurityScheme
                                            {
                                                Reference = new OpenApiReference
                                                {
                                                    Type = ReferenceType.SecurityScheme,
                                                    Id = "Public"
                                                }
                                            },

                                            new string[]{ }
                                        }
                                }
                        };
                        break;
                    case "api.admin":
                        operation.Security = new List<OpenApiSecurityRequirement>() {
                            new OpenApiSecurityRequirement
                            {
                                {
                                    new OpenApiSecurityScheme
                                    {
                                        Reference = new OpenApiReference
                                        {
                                            Type = ReferenceType.SecurityScheme,
                                            Id = "Admin"
                                        }
                                    },

                                    new string[]{ }
                                }
                            }
                        };
                        break;
                    default:
                        operation.Security = new List<OpenApiSecurityRequirement>();
                        break;
                }
            }
            else
            {
                operation.Security.Clear();
            }
        }
    }
}