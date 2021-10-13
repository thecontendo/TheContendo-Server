using Contendo.Data;
using Contendo.Data.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Contendo.Extensions.DataService
{
    public static class DataServiceRegistrationExtensions
    {
        public static void RegisterDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IShotService, ShotService>();
            services.AddTransient<IChallengeService, ChallengeService>();
            services.AddTransient<IContactRequestService, ContactRequestService>();
        }
    }
}