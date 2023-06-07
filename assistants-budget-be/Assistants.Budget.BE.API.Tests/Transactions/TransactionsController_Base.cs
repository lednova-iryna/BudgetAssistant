using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Assistants.Budget.BE.API.Tests.Mocks;
using Assistants.Budget.BE.BusinessLogic.Transactions.CQRS;
using Assistants.Budget.BE.Domain;
using Assistants.Budget.BE.Options;
using MongoDB.Driver;

namespace Assistants.Budget.BE.API.Tests.Transactions;

[Collection("TransactionsApi")]
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
        var accessToken = AuthModuleMock.GenerateJwtToken();
        appHttpClient.DefaultRequestHeaders.Authorization = new(JwtBearerHandlerMock.TestSchema, accessToken);
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
