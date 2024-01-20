using Application.Configuration;
using Application.Options;
using Application.Services;
using Application.Services.Interfaces;
using Application.Validators.Pokemon;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
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
        services.AddLogging();
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
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

        var connectionStringTemplate = configuration.GetConnectionString("DefaultTemplate")!;
        var connetionString = connectionStringTemplate
                                .Replace("{userName}", secretsOptions.DbUsername, StringComparison.InvariantCulture)
                                .Replace("{password}", secretsOptions.DbPassword, StringComparison.InvariantCulture);
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connetionString));
        return services;
    }

    private static IServiceCollection AddLogging(this IServiceCollection services)
    {
        services.AddScoped(typeof(ILoggerAdapter<>), typeof(ILoggerAdapter<>));

        // TODO: Configure SeriLog and Seq here
        
        return services;
    }
}
