using System.Collections.Concurrent;

namespace TatweerPOS.Helpers;

/// <summary>
/// In-memory cache with TTL expiration and version-based invalidation.
/// Thread-safe via ConcurrentDictionary.
/// </summary>
public static class CacheManager
{
    private static readonly ConcurrentDictionary<string, CacheEntry> _cache = new();

    /// <summary>
    /// Stores a value in the cache with a TTL and version tag.
    /// </summary>
    public static void Set<T>(string key, T data, TimeSpan ttl, string version)
    {
        var entry = new CacheEntry
        {
            Value = data!,
            Version = version,
            ExpiresAt = DateTime.UtcNow.Add(ttl)
        };

        _cache.AddOrUpdate(key, entry, (_, _) => entry);
    }

    /// <summary>
    /// Retrieves a value from the cache.
    /// Returns default if the key is missing, expired, or the version does not match.
    /// </summary>
    public static T? Get<T>(string key, string version)
    {
        if (!_cache.TryGetValue(key, out var entry))
            return default;

        if (DateTime.UtcNow >= entry.ExpiresAt || entry.Version != version)
        {
            _cache.TryRemove(key, out _);
            return default;
        }

        return entry.Value is T typed ? typed : default;
    }

    /// <summary>
    /// Removes a specific key from the cache.
    /// </summary>
    public static void Invalidate(string key)
    {
        _cache.TryRemove(key, out _);
    }

    /// <summary>
    /// Clears all entries from the cache.
    /// </summary>
    public static void Clear()
    {
        _cache.Clear();
    }

    internal class CacheEntry
    {
        public object Value { get; set; } = null!;
        public string Version { get; set; } = "";
        public DateTime ExpiresAt { get; set; }
    }
}
