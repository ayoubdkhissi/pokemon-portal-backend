using API.Middlewares;
using API.Services;

namespace API.Configuration;

public static class ConfigureApiServices
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddHealthChecks();

        services.AddSingleton<IResultHandler, ResultHandler>();
        services.AddScoped<GlobalExceptionHandlerMiddleware>();

        return services;
    }
}
