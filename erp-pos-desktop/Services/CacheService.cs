namespace TatweerPOS.Services;

using TatweerPOS.Helpers;

/// <summary>
/// Singleton cache service providing DI-friendly access to the static CacheManager.
/// Per CLAUDE.md: always specify TTL and version when caching data.
/// </summary>
public class CacheService
{
    /// <summary>
    /// Stores a value in the cache with a specified TTL and version.
    /// Version mismatches cause the cached entry to be treated as expired.
    /// </summary>
    public void Set<T>(string key, T data, TimeSpan ttl, string version = "v1")
    {
        CacheManager.Set(key, data, ttl, version);
    }

    /// <summary>
    /// Retrieves a cached value by key and version.
    /// Returns default(T) if not found, expired, or version mismatch.
    /// </summary>
    public T? Get<T>(string key, string version = "v1")
    {
        return CacheManager.Get<T>(key, version);
    }

    /// <summary>
    /// Removes a specific key from the cache.
    /// </summary>
    public void Invalidate(string key)
    {
        CacheManager.Invalidate(key);
    }

    /// <summary>
    /// Clears all entries from the cache.
    /// </summary>
    public void Clear()
    {
        CacheManager.Clear();
    }
}
