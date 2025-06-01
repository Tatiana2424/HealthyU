using HealthuU.BLL.Services.Interfaces.Cache;
using Microsoft.Extensions.Caching.Memory;
namespace HealthuU.BLL.Services.Realizations.Cache;

public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;
    private static readonly TimeSpan DefaultTtl = TimeSpan.FromMinutes(10);

    public MemoryCacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public T GetOrSet<T>(string key, Func<T> factory, TimeSpan? ttl = null)
    {
        if (_memoryCache.TryGetValue(key, out T cachedValue))
        {
            return cachedValue!;
        }

        var result = factory();
        if (result != null)
        {
            var actualTtl = ttl ?? DefaultTtl;
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = actualTtl,
                SlidingExpiration = TimeSpan.FromMinutes(2)
            };
            _memoryCache.Set(key, result, options);
        }
        return result;
    }

    public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? ttl = null)
    {
        if (_memoryCache.TryGetValue(key, out T cachedValue))
        {
            return cachedValue!;
        }

        var result = await factory();
        if (result != null)
        {
            var actualTtl = ttl ?? DefaultTtl;
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = actualTtl,
                SlidingExpiration = TimeSpan.FromMinutes(2)
            };
            _memoryCache.Set(key, result, options);
        }
        return result;
    }

    public void Invalidate(string key)
    {
        _memoryCache.Remove(key);
    }
}