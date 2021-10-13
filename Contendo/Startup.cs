using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contendo.Db.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Identity.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Vistex.Cloud.Services.Extensions.SwaggerService;
using Contendo.Db.Extensions.DbContext;
using Contendo.Extensions.AuthService;
using Contendo.Extensions.DataService;
using Microsoft.EntityFrameworkCore;

namespace Contendo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAd"));

            services.AddControllersWithViews()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                });
            
            services.AddHttpContextAccessor();
            services.RegisterSwaggerServices(Configuration);
            services.RegisterDataServices(Configuration);
            // services.RegisterMapperServices();
            services.AddCors(options =>
            {
                options.AddPolicy($"contendo-admin-ui-policy", builder =>
                {
                    //builder.WithOrigins(Configuration["VcsAdminUiUrl"])
                    builder
                        //.AllowAnyOrigin()
                        .SetIsOriginAllowed(_ => true)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
            
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedProto;
            });

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Information);
                builder.AddConsole();
                builder.AddEventSourceLogger();
            });
            services.RegisterDbServices(Configuration);
            services.RegisterAuthServices(Configuration, loggerFactory.CreateLogger("Startup"));
            services.AddMemoryCache();
            services.AddHealthChecks();
            services.AddLocalization();
            services.AddSignalR();
            
         
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/api.public/swagger.json", "Contendo Public");
                    c.SwaggerEndpoint("/swagger/api.admin/swagger.json", "Contendo Admin");
                });
                app.UseForwardedHeaders();
            }

            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetService<CDbContext>().Database.Migrate();
            }
            
            var supportedCultures = new[] { "en-US", "de-DE" };
            var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            app.UseRequestLocalization(localizationOptions);
            
            app.UseRouting();
            app.UseForwardedHeaders();
            
            
            var contentProvider = new FileExtensionContentTypeProvider();
            contentProvider.Mappings[".properties"] = "application/plaintext";
            contentProvider.Mappings[".json"] = "application/json";

            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = contentProvider
            });
            
            app.UseCors("contendo-admin-ui-policy");
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapHealthChecks("/health");
                //endpoints.MapHub<GlobalHub>("/hub");
            });
        }
    }
}