using Application.Services.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;

namespace Application.Configuration;
public static class ConfigureApplicationServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IService<>), typeof(Service<>));
        services.AddValidators();
        return services;
    }

    private static IServiceCollection AddValidators(this IServiceCollection services)
    {
        // Inject all DTO validators at once
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}
