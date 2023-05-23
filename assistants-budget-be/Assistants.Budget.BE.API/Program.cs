using Assistants.Aws.Parameters.Options;
using Assistants.Budget.BE.API.Configurators;
using Assistants.Budget.BE.Mediator;
using Assistants.Extensions.Options;
using Assistants.Libs.Aws.Parameters;
using Microsoft.Extensions.Configuration;
using Amazon.Extensions.NETCore.Setup;
using dotenv.net;
using Assistants.Budget.BE.Options;
using Assistants.Aws.Parameters.Constants;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureAppConfiguration(c =>
{
    DotEnv.Load();
    c.AddEnvironmentVariables();
    c.AddAwsParameterStore();
});

// Add services to the container.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
builder.Services.AddControllers();
builder.Services.AddMediator();
builder.Services.AddEndpointsApiExplorer();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwagger(true);

OptionsExtensions.LoadOptions<DatabaseOptions, DatabaseOptionsValidator>(builder.Configuration, builder.Services);

var app = builder.Build();

if(AwsParametersConnectionState.ConnectionState != SecretsManagerConnectionStateEnum.Connected)
{
    throw AwsParametersConnectionState.ConnectionException!;
}

app.UseSwagger(true);
app.UseAuthorization();
app.MapControllers();

app.MapGet(
    "/",
    async context =>
    {
        if (true)
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
