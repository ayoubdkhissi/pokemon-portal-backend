using API.Middlewares;
using API.Services;
using Application.Common;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace API.Configuration;

public static class ConfigureApiServices
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        var apiName = configuration.GetValue<string>("App:ApiName");

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = apiName, Version = "v1" });
            c.IncludeXmlCommentsForAssembly(typeof(OperationResult).Assembly);
            c.IncludeXmlCommentsForAssembly(Assembly.GetExecutingAssembly());
        });
        services.AddHealthChecks();

        services.AddSingleton<IResultHandler, ResultHandler>();
        services.AddScoped<GlobalExceptionHandlerMiddleware>();

        return services;
    }


    public static void IncludeXmlCommentsForAssembly(this SwaggerGenOptions options, Assembly assembly)
    {
        var xmlFileName = $"{assembly.GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
        options.IncludeXmlComments(xmlPath);
    }
}
