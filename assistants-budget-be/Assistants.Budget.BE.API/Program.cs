using System.Diagnostics.CodeAnalysis;
using Assistants.Budget.BE.API.Configurators;
using Assistants.Budget.BE.API.Middlewares;
using Assistants.Budget.BE.API.Services;
using Assistants.Budget.BE.Modules.Core;
using Assistants.Budget.BE.Modules.Database;
using Assistants.Budget.BE.Modules.Database.Options;
using Assistants.Budget.BE.Modules.Transactions;
using Assistants.Budget.BE.Options;
using Assistants.Extensions.Options;
using Assistants.Libs.Aws.Parameters;
using Assistants.Libs.Aws.Parameters.Constants;
using Assistants.Libs.Caching.MongoDistributedCache;
using dotenv.net;
using Newtonsoft.Json.Converters;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureAppConfiguration(c =>
{
    DotEnv.Load();
    c.AddEnvironmentVariables();
    c.AddAwsParameterStore();
});

// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-7.0
builder.Services.AddProblemDetails();
builder.Services.AddMemoryCache();

// Add services to the container.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddTransactionsModule();
builder.Services.AddScoped<IRequestIdentityService, RequestIdentityService>();
builder.Services.AddAuth(builder.Configuration);

builder.Services
    .AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();

var databaseOptions = OptionsExtensions.LoadOptions<DatabaseOptions, DatabaseOptions.Validator>(
    builder.Configuration,
    builder.Services
);
var generalOptions = OptionsExtensions.LoadOptions<GeneralOptions, GeneralOptions.Validator>(
    builder.Configuration,
    builder.Services
);

builder.Services.AddMongoDistributedCache(x =>
{
    x.ConnectionString = databaseOptions.ConnectionString;
    x.DatabaseName = databaseOptions.Name;
    x.CollectionName = "appcache";
    x.ExpiredScanInterval = TimeSpan.FromDays(1);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwagger(builder.Configuration);

var app = builder.Build();

if (AwsParametersConnectionState.ConnectionState != SecretsManagerConnectionStateEnum.Connected)
{
    throw AwsParametersConnectionState.ConnectionException!;
}
app.UseMiddleware<ValidationExceptionMiddleware>();
app.UseSwagger(generalOptions.IsSwaggerEnabled);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.MapGet(
    "/",
    async context =>
    {
        if (generalOptions.IsSwaggerEnabled)
        {
            context.Response.Redirect("/swagger/index.html");
        }
        else
        {
            await context.Response.WriteAsync(string.Empty);
        }
    }
);

app.Run();

// Partial Program class needed for tests.
public partial class Program { }
