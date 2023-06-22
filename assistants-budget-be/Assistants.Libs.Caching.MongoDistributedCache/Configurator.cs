using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace Assistants.Libs.Caching.MongoDistributedCache;

public static class Configurator
{
    public static IServiceCollection AddMongoDistributedCache(
        this IServiceCollection services,
        Action<MongoDistributedCacheOptions> setupAction
    )
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        if (setupAction == null)
            throw new ArgumentNullException(nameof(setupAction));

        services.Configure(setupAction);
        services.AddSingleton<IDistributedCache, MongoDistributedCache>();

        return services;
    }
}
