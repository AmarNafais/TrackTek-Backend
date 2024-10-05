using Microsoft.Extensions.DependencyInjection;
using Services;
using Data.Repositories;

namespace Web
{
    public static class ServiceConfigurator
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            // Services
            services.AddScoped<IUserService, UserService>();
            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
