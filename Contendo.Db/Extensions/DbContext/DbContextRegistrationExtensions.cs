using Contendo.Db.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Contendo.Db.Extensions.DbContext
{
    public static class DbContextRegistrationExtensions
    {
        public static void RegisterDbServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CDbContext>(options => {
                options.UseNpgsql(configuration.GetConnectionString("main"));
            });
        }
    }
}