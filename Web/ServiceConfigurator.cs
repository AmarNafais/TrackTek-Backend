﻿using Data.Repositories;
using Data.Entities;
using Service;

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
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IMachineService, MachineService>();
            services.AddScoped<IMaterialService, MaterialService>();
            services.AddScoped<IGarmentService, GarmentService>();

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IMachineRepository, MachineRepository>();
            services.AddScoped<IMaterialRepository, MaterialRepository>();
            services.AddScoped<IGarmentRepository, GarmentRepository>();

            // Configure SmtpSettings
            services.Configure<SMTPSetting>(services.BuildServiceProvider().GetService<IConfiguration>().GetSection("SmtpSettings"));
        }
    }
}