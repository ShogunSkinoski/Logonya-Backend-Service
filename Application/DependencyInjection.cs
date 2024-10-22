using Application.Common;
using Domain.Common;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        services.AddValidatorsFromAssembly(assembly);
        services.AddTransient<ITokenRotationPolicy, DefaultTokenRotationPolicy>(provider => new DefaultTokenRotationPolicy(TimeSpan.FromDays(7)));

        // If you're using AutoMapper, you can add it here as well
        // services.AddAutoMapper(assembly);

        return services;
    }
}
