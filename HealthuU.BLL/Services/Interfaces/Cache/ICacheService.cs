namespace HealthuU.BLL.Services.Interfaces.Cache;

public interface ICacheService
{
    T GetOrSet<T>(string key, Func<T> factory, TimeSpan? ttl = null);
    Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? ttl = null);
    void Invalidate(string key);
}
