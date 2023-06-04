using Assistants.Budget.BE.Options;
using Assistants.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Assistants.Budget.BE.MongoDB;

public static class Configurator
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var options = OptionsExtensions.LoadOptions<DatabaseOptions, DatabaseOptionsValidator>(configuration, services);
        var settings = MongoClientSettings.FromConnectionString(options.ConnectionString);
        settings.ServerApi = new ServerApi(ServerApiVersion.V1);

        return services.AddScoped<MongoClient>((serviceProvider) => new MongoClient(settings));
    }
}


