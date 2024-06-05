using Application.Services.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Infrastructure.Services;
public class CacheService : ICacheService
{
    private readonly ConnectionMultiplexer _connection;

    public CacheService(string connectionString)
    {
        _connection = ConnectionMultiplexer.Connect(connectionString);
    }


    public async Task<T?> GetAsync<T>(string key)
    {
        var database = _connection.GetDatabase();
        var value = await database.StringGetAsync(key);
        return value.HasValue ? JsonConvert.DeserializeObject<T>(value!) : default;
    }

    public async Task SetAsync(string key, string value, int expirationInSeconds)
    {
        var database = _connection.GetDatabase();
        await database.StringSetAsync(key, value, TimeSpan.FromSeconds(expirationInSeconds));
    }
}