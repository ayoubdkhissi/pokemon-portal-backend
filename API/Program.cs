using API.Middlewares;
using Infrastructure.Configuration;
using Application.Configuration;
using API.Configuration;
var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();


builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration, builder.Environment.EnvironmentName);
builder.Services.AddApplicationServices();

var app = builder.Build();


// Configure the HTTP request pipeline.
var devEnvs = new[] { "Local" };
if (devEnvs.Contains(app.Environment.EnvironmentName))
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(c =>
{
    c.WithOrigins(builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>());
});

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseHealthChecks("/health");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
await app.RunAsync();