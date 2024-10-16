using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Domain.Entities;
using Domain.Common.Entities;
using Application.Common.Models;
using System;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register DbContext based on environment configuration (e.g., PostgreSQL or SQL Server)
            var Onliinedb = bool.Parse(configuration["Onliinedb"] ?? "false");
            
            if (Onliinedb)
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("SmarterAspConnection")));

            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                        sqlOptions =>
                        {
                            sqlOptions.EnableRetryOnFailure(
                                maxRetryCount: 5, // Retry attempts
                                maxRetryDelay: TimeSpan.FromSeconds(10), // Delay between retries
                                errorNumbersToAdd: null // Optional error numbers
                            );
                        }));
            }


            services.AddSingleton<IEmailService>(provider =>
            {
                return new EmailService(configuration);
            });
            
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

            // Register Identity for StudentController
            services.AddIdentityCore<User>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddApiEndpoints();
            
        

            var accesskey = configuration["Jwt:AccessKey"];
            var refreshkey =configuration["Jwt:RefreshKey"];
            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];

            services.AddScoped<IGenerateToken, GenerateTokenService>(provider =>
            {
                return new GenerateTokenService(accesskey, refreshkey, issuer, audience);
            });

            //services.AddSingleton<TaskNotificationService>();

            services.AddHostedService<TaskNotificationService>();
            return services;
        }
    }
}
