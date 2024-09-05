using Application.Common.Models;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using CloudinaryDotNet;
using Microsoft.Extensions.Configuration;
using Serilog;
using StackExchange.Redis;

//using Application.AuthController;


namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            Assembly assembly = typeof(DependencyInjection).Assembly;

            services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));
            
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                });

            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(configuration.GetConnectionString("redisConnection")));
        
            // Register Cloudinary
            var cloudinaryConfig = configuration.GetSection("Cloudinary").Get<CloudinaryDotNet.Account>();
            var cloudinary = new Cloudinary(cloudinaryConfig);
            services.AddSingleton(cloudinary);

            services.AddScoped<Result>();
            services.AddLogging();
            


            return services;
        }
    }
}