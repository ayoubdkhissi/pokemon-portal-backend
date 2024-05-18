using Application.Services;
using Application.Services.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Configuration;
public static class ConfigureApplicationServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IService<>), typeof(Service<>));
        services.AddScoped<IPokemonService, PokemonService>();
        services.AddValidators();
        return services;
    }

    private static IServiceCollection AddValidators(this IServiceCollection services)
    {
        // Inject all DTO validators at once
        var validatorTypes = typeof(ConfigureApplicationServices).Assembly.GetTypes()
            .Where(t => t.IsClass &&
            !t.IsAbstract &&
            !t.IsGenericTypeDefinition &&
            t.Name.EndsWith("Validator", StringComparison.InvariantCulture));

        foreach (var validatorType in validatorTypes)
        {
            var validatorInterface = validatorType.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType &&
                (i.GetGenericTypeDefinition() == typeof(ICreateValidator<>) ||
                 i.GetGenericTypeDefinition() == typeof(IUpdateValidator<>)));
            if (validatorInterface is null)
                continue;

            services.AddScoped(validatorInterface, validatorType);
        }

        return services;
    }
}
