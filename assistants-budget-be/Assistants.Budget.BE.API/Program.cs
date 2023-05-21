using Assistants.Budget.BE.API.Configurators;
using Assistants.Budget.BE.Mediator;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddMediator();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger(true);

var app = builder.Build();

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
