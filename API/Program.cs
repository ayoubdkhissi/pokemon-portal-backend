using API.Middlewares;
using Infrastructure.Configuration;
using Application.Configuration;
using API.Configuration;
using Azure.Identity;
using Infrastructure.Persistence;
var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

if (builder.Environment.IsEnvironment("Local"))
{
    builder.Configuration.AddUserSecrets<Program>();
}
else
{
    builder.Configuration.AddAzureKeyVault(new Uri(builder.Configuration["KeyVault:BaseUrl"]!), new DefaultAzureCredential());
}

builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
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

    // init db
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DbInitializer>();
    await context.InitializeAsync();
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