using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Assistants.Budget.BE.API.Tests.Mocks;
using Assistants.Budget.BE.Modules.Database.Options;
using Assistants.Budget.BE.Modules.Auth.Models;
using Assistants.Budget.BE.Modules.Transactions.CQRS;
using Assistants.Budget.BE.Modules.Transactions.Domain;
using Assistants.Libs.AspNetCore.Auth.Permissions;
using MongoDB.Driver;

namespace Assistants.Budget.BE.API.Tests.Transactions;

[Collection("APITests")]
public partial class TransactionsController : IClassFixture<WebAppFactoryMock<Program>>, IDisposable
{
    private readonly WebAppFactoryMock<Program> appFactory;
    private readonly HttpClient appHttpClient;
    private readonly IServiceScope scope;
    private readonly MongoClient mongoClient;
    private readonly DatabaseOptions databaseOptions;
    private readonly string rootUrl = "/transactions";

    public TransactionsController(WebAppFactoryMock<Program> factory)
    {
        appFactory = factory;
        appHttpClient = factory.CreateClient();

        scope = appFactory.Services.CreateScope();
        mongoClient = scope.ServiceProvider.GetService<MongoClient>()!;
        databaseOptions = scope.ServiceProvider.GetService<IOptions<DatabaseOptions>>()!.Value;

        mongoClient.DropDatabase(databaseOptions.Name);

        // Setup default AccessToken into requests
        var accessToken = AuthModuleMock.GenerateJwtToken(
            new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Test User Transactions"),
                new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()),
                new Claim(AssistantsClaimTypes.Permissions, IdentityPermissions.TransactionCanCreate),
                new Claim(AssistantsClaimTypes.Permissions, IdentityPermissions.TransactionCanDelete),
                new Claim(AssistantsClaimTypes.Permissions, IdentityPermissions.TransactionCanEdit),
                new Claim(AssistantsClaimTypes.Permissions, IdentityPermissions.TransactionCanRead)
            }
        );
        appHttpClient.DefaultRequestHeaders.Authorization = new(JwtBearerDefaults.AuthenticationScheme, accessToken);
    }

    public static IEnumerable<object[]> validCreateTransactionCommandSet = new List<TransactionsCreateCommand[]>
    {
        new[]
        {
            new TransactionsCreateCommand
            {
                Type = TransactionType.Income,
                Amount = 10,
                Date = DateTime.UtcNow,
                Note = "Test Note"
            }
        },
        new[]
        {
            new TransactionsCreateCommand
            {
                Type = TransactionType.Expense,
                Amount = 10,
                Date = DateTime.UtcNow,
                Note = "Test Note"
            }
        }
    };

    public static IEnumerable<object[]> invalidCreateTransactionCommandSet = new List<TransactionsCreateCommand[]>
    {
        new[]
        {
            new TransactionsCreateCommand
            {
                Type = TransactionType.Income,
                Date = new DateTime(),
                Note = "Test Note"
            }
        },
        new[]
        {
            new TransactionsCreateCommand
            {
                Type = TransactionType.Income,
                Amount = 10,
                Note = "Test Note"
            }
        },
        new[]
        {
            new TransactionsCreateCommand { Type = TransactionType.Income, Note = "Test Note" }
        },
        new[]
        {
            new TransactionsCreateCommand { Date = new DateTime(), Note = "Test Note" }
        }
    };

    public async void Dispose()
    {
        await mongoClient.DropDatabaseAsync(databaseOptions.Name);
    }
}
