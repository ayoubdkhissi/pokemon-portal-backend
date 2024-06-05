using Application.Services.Interfaces;

namespace Infrastructure.Services;
public class FakeCacheService : ICacheService
{
    public async Task<T?> GetAsync<T>(string key)
    {
        return await Task.FromResult<T?>(default);
    }

    public async Task SetAsync(string key, string value, int expirationInSeconds)
    {
        await Task.CompletedTask;
    }
}
