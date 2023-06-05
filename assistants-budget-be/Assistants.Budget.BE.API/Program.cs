using Assistants.Aws.Parameters.Options;
using Assistants.Budget.BE.API.Configurators;
using Assistants.Budget.BE.BusinessLogic;
using Assistants.Extensions.Options;
using Assistants.Libs.Aws.Parameters;
using Microsoft.Extensions.Configuration;
using Amazon.Extensions.NETCore.Setup;
using dotenv.net;
using Assistants.Budget.BE.Options;
using Assistants.Aws.Parameters.Constants;
using Assistants.Budget.BE.MongoDB;
using Assistants.Budget.BE.API.Middlewares;
using Assistants.Budget.BE.Auth0;

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
builder.Services.AddAuth0Module(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddBusinessLogic();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDatabase(builder.Configuration);

var databaseOptions = OptionsExtensions.LoadOptions<DatabaseOptions, DatabaseOptions.Validator>(builder.Configuration, builder.Services);
var generalOptions = OptionsExtensions.LoadOptions<GeneralOptions, GeneralOptions.Validator>(builder.Configuration, builder.Services);

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
public partial class Program
{ }