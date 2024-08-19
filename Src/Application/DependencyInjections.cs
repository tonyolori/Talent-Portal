using Application.Common.Models;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Serilog;
//using Application.Users;


namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            Assembly assembly = typeof(DependencyInjection).Assembly;

            services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));

            //services.AddValidatorsFromAssembly(assembly);

            services.AddScoped<Result>();
            services.AddLogging();
            


            return services;
        }
    }
}