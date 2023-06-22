using Microsoft.Extensions.Caching.Distributed;
using Assistants.Libs.Caching.MongoDistributedCache.Domain;
using MongoDB.Driver;

namespace Assistants.Libs.Caching.MongoDistributedCache;

class MongoDbClient
{
    private readonly IMongoCollection<CacheEntry> collection;

    private static FilterDefinition<CacheEntry> FilterByKey(string key) =>
        Builders<CacheEntry>.Filter.Eq(x => x.Key, key);

    private static FilterDefinition<CacheEntry> FilterByExpiresAtNotNull() =>
        Builders<CacheEntry>.Filter.Ne(x => x.ExpiresAt, null);

    private IFindFluent<CacheEntry, CacheEntry> GetItemQuery(string key, bool withoutValue)
    {
        var query = collection.Find(FilterByKey(key));
        if (withoutValue)
            query = query.Project<CacheEntry>(Builders<CacheEntry>.Projection.Exclude(x => x.Value));

        return query;
    }

    private static bool CheckIfExpired(DateTimeOffset utcNow, CacheEntry cacheItem) => cacheItem?.ExpiresAt <= utcNow;

    private static DateTimeOffset? GetExpiresAt(
        DateTimeOffset utcNow,
        double? slidingExpirationInSeconds,
        DateTimeOffset? absoluteExpiration
    )
    {
        if (slidingExpirationInSeconds == null && absoluteExpiration == null)
            return null;

        if (slidingExpirationInSeconds == null)
            return absoluteExpiration;

        var seconds = slidingExpirationInSeconds.GetValueOrDefault();

        return utcNow.AddSeconds(seconds) > absoluteExpiration ? absoluteExpiration : utcNow.AddSeconds(seconds);
    }

    private CacheEntry UpdateExpiresAtIfRequired(DateTimeOffset utcNow, CacheEntry cacheItem)
    {
        if (cacheItem.ExpiresAt == null)
            return cacheItem;

        var absoluteExpiration = GetExpiresAt(
            utcNow,
            cacheItem.SlidingExpirationInSeconds,
            cacheItem.AbsoluteExpiration
        );
        collection.UpdateOne(
            FilterByKey(cacheItem.Key) & FilterByExpiresAtNotNull(),
            Builders<CacheEntry>.Update.Set(x => x.ExpiresAt, absoluteExpiration)
        );

        return cacheItem.WithExpiresAt(absoluteExpiration);
    }

    private async Task<CacheEntry> UpdateExpiresAtIfRequiredAsync(DateTimeOffset utcNow, CacheEntry cacheItem)
    {
        if (cacheItem.ExpiresAt == null)
            return cacheItem;

        var absoluteExpiration = GetExpiresAt(
            utcNow,
            cacheItem.SlidingExpirationInSeconds,
            cacheItem.AbsoluteExpiration
        );
        await collection.UpdateOneAsync(
            FilterByKey(cacheItem.Key) & FilterByExpiresAtNotNull(),
            Builders<CacheEntry>.Update.Set(x => x.ExpiresAt, absoluteExpiration)
        );

        return cacheItem.WithExpiresAt(absoluteExpiration);
    }

    public MongoDbClient(
        string connectionString,
        MongoClientSettings mongoClientSettings,
        string databaseName,
        string collectionName
    )
    {
        var client =
            mongoClientSettings == null ? new MongoClient(connectionString) : new MongoClient(mongoClientSettings);
        var database = client.GetDatabase(databaseName);

        var expireAtIndexModel = new IndexKeysDefinitionBuilder<CacheEntry>().Ascending(p => p.ExpiresAt);

        collection = database.GetCollection<CacheEntry>(collectionName);

        collection.Indexes.CreateOne(
            new CreateIndexModel<CacheEntry>(expireAtIndexModel, new CreateIndexOptions { Background = true })
        );
    }

    public void DeleteExpired(DateTimeOffset utcNow) =>
        collection.DeleteMany(Builders<CacheEntry>.Filter.Lte(x => x.ExpiresAt, utcNow));

    public byte[] GetCacheEntry(string key, bool withoutValue)
    {
        var utcNow = DateTimeOffset.UtcNow;

        if (key == null)
            return null;

        var query = GetItemQuery(key, withoutValue);
        var cacheItem = query.SingleOrDefault();
        if (cacheItem == null)
            return null;

        if (CheckIfExpired(utcNow, cacheItem))
        {
            Remove(cacheItem.Key);
            return null;
        }

        cacheItem = UpdateExpiresAtIfRequired(utcNow, cacheItem);

        return cacheItem?.Value;
    }

    public async Task<byte[]> GetCacheEntryAsync(string key, bool withoutValue, CancellationToken token = default)
    {
        var utcNow = DateTimeOffset.UtcNow;

        if (key == null)
            return null;

        var query = GetItemQuery(key, withoutValue);
        var cacheItem = await query.SingleOrDefaultAsync(token);
        if (cacheItem == null)
            return null;

        if (CheckIfExpired(utcNow, cacheItem))
        {
            await RemoveAsync(cacheItem.Key, token);
            return null;
        }

        cacheItem = await UpdateExpiresAtIfRequiredAsync(utcNow, cacheItem);

        return cacheItem?.Value;
    }

    public void Set(string key, byte[] value, DistributedCacheEntryOptions? options = null)
    {
        var utcNow = DateTimeOffset.UtcNow;

        if (key == null)
            throw new ArgumentNullException(nameof(key));

        if (value == null)
            throw new ArgumentNullException(nameof(value));

        var absolutExpiration = options?.AbsoluteExpiration;
        var slidingExpirationInSeconds = options?.SlidingExpiration?.TotalSeconds;

        if (options?.AbsoluteExpirationRelativeToNow != null)
            absolutExpiration = utcNow.Add(options.AbsoluteExpirationRelativeToNow.Value);

        if (absolutExpiration <= utcNow)
            throw new InvalidOperationException("The absolute expiration value must be in the future.");

        var expiresAt = GetExpiresAt(utcNow, slidingExpirationInSeconds, absolutExpiration);
        var cacheItem = new CacheEntry(key, value, expiresAt, absolutExpiration, slidingExpirationInSeconds);

        collection.ReplaceOne(FilterByKey(key), cacheItem, new ReplaceOptions { IsUpsert = true });
    }

    public async Task SetAsync(
        string key,
        byte[] value,
        DistributedCacheEntryOptions? options = null,
        CancellationToken token = default
    )
    {
        var utcNow = DateTimeOffset.UtcNow;

        if (key == null)
            throw new ArgumentNullException(nameof(key));

        if (value == null)
            throw new ArgumentNullException(nameof(value));

        var absolutExpiration = options?.AbsoluteExpiration;
        var slidingExpirationInSeconds = options?.SlidingExpiration?.TotalSeconds;

        if (options?.AbsoluteExpirationRelativeToNow != null)
            absolutExpiration = utcNow.Add(options.AbsoluteExpirationRelativeToNow.Value);

        if (absolutExpiration <= utcNow)
            throw new InvalidOperationException("The absolute expiration value must be in the future.");

        var expiresAt = GetExpiresAt(utcNow, slidingExpirationInSeconds, absolutExpiration);
        var cacheItem = new CacheEntry(key, value, expiresAt, absolutExpiration, slidingExpirationInSeconds);

        await collection.ReplaceOneAsync(FilterByKey(key), cacheItem, new ReplaceOptions { IsUpsert = true }, token);
    }

    public void Remove(string key) => collection.DeleteOne(FilterByKey(key));

    public async Task RemoveAsync(string key, CancellationToken token = default) =>
        await collection.DeleteOneAsync(FilterByKey(key), token);
}
