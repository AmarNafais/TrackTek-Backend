using Data.Repositories;
using Data.Entities;

namespace Web
{
    public static class ServiceConfigurator
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            // Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAuthService, AuthService>();

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();

            // Configure SmtpSettings
            services.Configure<SMTPSetting>(services.BuildServiceProvider().GetService<IConfiguration>().GetSection("SmtpSettings"));
        }
    }
}