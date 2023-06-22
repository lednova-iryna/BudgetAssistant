using MongoDB.Bson.Serialization.Attributes;

namespace Assistants.Libs.Caching.MongoDistributedCache.Domain;

internal class CacheEntry
{
    [BsonId]
    public string Key { get; }

    [BsonElement("value")]
    public byte[] Value { get; }

    [BsonElement("exp")]
    public DateTimeOffset? ExpiresAt { get; set; }

    [BsonElement("abs")]
    public DateTimeOffset? AbsoluteExpiration { get; }

    [BsonElement("sli")]
    public double? SlidingExpirationInSeconds { get; }

    [BsonConstructor]
    public CacheEntry(
        string key,
        byte[] value,
        DateTimeOffset? expiresAt,
        DateTimeOffset? absoluteExpiration,
        double? slidingExpirationInSeconds
    )
    {
        Key = key;
        Value = value;
        ExpiresAt = expiresAt;
        AbsoluteExpiration = absoluteExpiration;
        SlidingExpirationInSeconds = slidingExpirationInSeconds;
    }

    [BsonConstructor]
    public CacheEntry(
        string key,
        DateTimeOffset? expiresAt,
        DateTimeOffset? absoluteExpiration,
        double? slidingExpirationInSeconds
    )
        : this(key, null, expiresAt, absoluteExpiration, slidingExpirationInSeconds) { }

    public CacheEntry WithExpiresAt(DateTimeOffset? expiresAt)
    {
        ExpiresAt = expiresAt;
        return this;
    }
}
