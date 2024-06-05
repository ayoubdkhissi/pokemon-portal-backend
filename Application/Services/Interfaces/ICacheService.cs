namespace Application.Services.Interfaces;
public interface ICacheService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync(string key, string value, int expirationInSeconds);
}
