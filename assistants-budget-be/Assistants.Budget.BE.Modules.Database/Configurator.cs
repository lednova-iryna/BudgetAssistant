using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Assistants.Budget.BE.Modules.Database.Options;
using Assistants.Extensions.Options;
using MongoDB.Driver;

namespace Assistants.Budget.BE.Modules.Database;

public static class Configurator
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var options = OptionsExtensions.LoadOptions<DatabaseOptions, DatabaseOptions.Validator>(
            configuration,
            services
        );
        var settings = MongoClientSettings.FromConnectionString(options.ConnectionString);
        settings.ServerApi = new ServerApi(ServerApiVersion.V1);

        return services.AddScoped<MongoClient>((serviceProvider) => new MongoClient(settings));
    }
}
