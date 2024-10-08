﻿using Application.Services.Interfaces;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Constants;

namespace Infrastructure.Configuration;
public static class ConfigureInfraServices
{

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationOptions(configuration);
        services.AddDatabase(configuration);
        services.AddLogging();
        services.AddRepositories();
        services.AddCaching(configuration);
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRepositoryFactory, RepositoryFactory>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IPokemonRepository, PokemonRepository>();
        return services;
    }

    private static IServiceCollection AddApplicationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        // Configure Options pattern Here

        //services.Configure<SecretsOptions>(options =>
        //{
        //    options.DbUsername = configuration[$"DbUsername"]!;
        //    options.DbPassword = configuration[$"DbPassword"]!;
        //});

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionStringTemplate = configuration.GetConnectionString("SqlServerTemplate")!;
        var connetionString = connectionStringTemplate
                                .Replace("{userName}", configuration["DbUsername"]!)
                                .Replace("{password}", configuration["DbPassword"]!);

        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connetionString));
        services.AddScoped<DbInitializer>();
        return services;
    }

    private static IServiceCollection AddLogging(this IServiceCollection services)
    {
        services.AddScoped(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));

        // TODO: Configure SeriLog and Seq here

        return services;
    }

    private static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var cachingEnabled = string.Equals(Environment.GetEnvironmentVariable("ENABLE_CACHING"), "1", StringComparison.OrdinalIgnoreCase);
        if (!cachingEnabled)
        {
            services.AddSingleton<ICacheService, FakeCacheService>();
            return services;
        }
        var connectionStringTemplate = configuration.GetConnectionString("RedisTemplate")!;
        var connetionString = connectionStringTemplate
                                .Replace("{host}", configuration["RedisHost"]!, StringComparison.InvariantCulture)
                                .Replace("{password}", configuration["RedisPassword"]!, StringComparison.InvariantCulture);

        services.AddSingleton<ICacheService>(sp => new CacheService(connetionString));
        return services;
    }
}
