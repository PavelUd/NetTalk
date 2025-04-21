using Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure;

public class CacheService(IMemoryCache memoryCache) : ICacheService
{
    public TItem? Get<TItem>(Guid key)
    {
        return  memoryCache.Get<TItem>(key);
    }
    
    public void Set<TItem>(Guid key, in TItem value)
    {
        memoryCache.Set(key, value);
    }
}