using Application.Configuration;
using Application.Options;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.Configuration;
public static class ConfigureInfraServices
{
    
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, string envName)
    {
        services.AddApplicationOptions(configuration, envName);
        services.AddDatabase(configuration);

        return services;
    }


    
    private static IServiceCollection AddApplicationOptions(this IServiceCollection services, IConfiguration configuration, string envName)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        services.Configure<AzureAdOptions>(configuration.GetSection("AzureAd"));
        
        services.Configure<SecretsOptions>(options => { });
        services.AddTransient<IConfigureOptions<SecretsOptions>>(provider => 
            new SecretsOptionsConfigure(provider.GetRequiredService<ISecretsService>(), envName));

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var secretsOptions = services.BuildServiceProvider().GetService<IOptions<SecretsOptions>>()?.Value!;

        var connectionStringTemplate = configuration.GetConnectionString("Default")!;
        var connetionString = connectionStringTemplate
                                .Replace("{userName}", secretsOptions.DbUsername, StringComparison.InvariantCulture)
                                .Replace("{password}", secretsOptions.DbPassword, StringComparison.InvariantCulture);
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connetionString));
        return services;
    }
}
