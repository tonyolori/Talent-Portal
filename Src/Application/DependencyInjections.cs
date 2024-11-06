using Application.Common.Models;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using CloudinaryDotNet;
using Microsoft.Extensions.Configuration;
using Serilog;
using StackExchange.Redis;
using Application.Paystack.Commands;

//using Application.AuthController;


namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            Assembly assembly = typeof(DependencyInjection).Assembly;

            services.AddAutoMapper(typeof(MappingProfile));
            services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));
            services.AddHttpClient<CreatePaymentCommandHandler>();
            services.AddHttpClient<VerifyPaymentCommandHandler>();
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