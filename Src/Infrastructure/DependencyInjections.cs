using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Mail;
using Application.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Domain.Entities;
using Domain.Common.Entities;


namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            


            services.AddSingleton<IEmailService>(provider =>
            {
                return new EmailService(configuration);
            });


            //Identity
            services.AddIdentityCore<Student>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddApiEndpoints();
            //Identity
            services.AddIdentityCore<Teacher>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddApiEndpoints();

            //Identity
            services.AddIdentityCore<LearningAdmin>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddApiEndpoints();



            var key = configuration["Jwt:Key"];
            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];

            services.AddScoped<IGenerateToken, GenerateTokenService>(provider =>
            {
                
                return new (key, issuer, audience);
            });

            

            return services;
        }
    }
}