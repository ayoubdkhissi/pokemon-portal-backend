using Application.Configuration;
using Application.Options;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.Configuration;
public static class ConfigureInfraServices
{
    
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, string envName)
    {
        services.AddApplicationOptions(configuration, envName);

        return services;
    }


    
    private static IServiceCollection AddApplicationOptions(this IServiceCollection services, IConfiguration configuration, string envName)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        services.Configure<AzureAdOptions>(configuration.GetSection("AzureAd"));
        
        services.Configure<SecretOptions>(options => { });
        services.AddTransient<IConfigureOptions<SecretOptions>>(provider => 
            new SecretsOptionsConfigure(provider.GetRequiredService<ISecretsService>(), envName));

        return services;
    }
}
