using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace Assistants.Libs.Caching.MongoDistributedCache;

public class MongoDistributedCache : IDistributedCache
{
    private DateTimeOffset lastScan = DateTimeOffset.UtcNow;
    private TimeSpan scanInterval;
    private readonly TimeSpan defaultScanInterval = TimeSpan.FromMinutes(5);
    private readonly MongoDbClient mongoDbClient;

    private void SetScanInterval(TimeSpan? scanInterval)
    {
        this.scanInterval = scanInterval?.TotalSeconds > 0 ? scanInterval.Value : defaultScanInterval;
    }

    public MongoDistributedCache(IOptions<MongoDistributedCacheOptions> optionsAccessor)
    {
        var options = optionsAccessor.Value;
        new MongoDistributedCacheOptions.Validator().Validate(options);

        mongoDbClient = new MongoDbClient(
            options.ConnectionString,
            options.MongoClientSettings,
            options.DatabaseName,
            options.CollectionName
        );

        SetScanInterval(options.ExpiredScanInterval);
    }

    public byte[] Get(string key)
    {
        var value = mongoDbClient.GetCacheEntry(key, withoutValue: false);

        ScanAndDeleteExpired();

        return value;
    }

    public void Set(string key, byte[] value, DistributedCacheEntryOptions options = null)
    {
        mongoDbClient.Set(key, value, options);

        ScanAndDeleteExpired();
    }

    public void Refresh(string key)
    {
        mongoDbClient.GetCacheEntry(key, withoutValue: true);

        ScanAndDeleteExpired();
    }

    public async Task<byte[]> GetAsync(string key, CancellationToken token = default)
    {
        var value = await mongoDbClient.GetCacheEntryAsync(key, withoutValue: false, token: token);

        ScanAndDeleteExpired();

        return value;
    }

    public async Task SetAsync(
        string key,
        byte[] value,
        DistributedCacheEntryOptions options,
        CancellationToken token = default
    )
    {
        await mongoDbClient.SetAsync(key, value, options, token);

        ScanAndDeleteExpired();
    }

    public async Task RefreshAsync(string key, CancellationToken token = default)
    {
        await mongoDbClient.GetCacheEntryAsync(key, withoutValue: true, token: token);

        ScanAndDeleteExpired();
    }

    public async Task RemoveAsync(string key, CancellationToken token = default)
    {
        await mongoDbClient.RemoveAsync(key, token);

        ScanAndDeleteExpired();
    }

    public void Remove(string key)
    {
        mongoDbClient.Remove(key);

        ScanAndDeleteExpired();
    }

    private void ScanAndDeleteExpired()
    {
        var utcNow = DateTimeOffset.UtcNow;

        if (lastScan.Add(scanInterval) < utcNow)
            Task.Run(() =>
            {
                lastScan = utcNow;
                mongoDbClient.DeleteExpired(utcNow);
            });
    }
}
