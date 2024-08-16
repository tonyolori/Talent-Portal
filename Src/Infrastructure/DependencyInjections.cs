using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Mail;
using Application.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            

            services.AddSingleton<SmtpClient>();
            
            
            
            services.AddScoped<IGenerateToken, GenerateTokenServive>(provider =>
            {
                var key = configuration["Jwt:Key"];
                var issuer = configuration["Jwt:Issuer"];
                var audience = configuration["Jwt:Audience"];
                return new (key, issuer, audience);
            });

            

            return services;
        }
    }
}