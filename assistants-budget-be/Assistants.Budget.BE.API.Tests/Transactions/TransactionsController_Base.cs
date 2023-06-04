using System.Net.Http.Json;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Assistants.Budget.BE.Domain;
using Assistants.Budget.BE.BusinessLogic.Transactions.CQRS;
using System.Net;
using Assistants.Budget.BE.API.Tests.Helpers;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Assistants.Budget.BE.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Assistants.Budget.BE.API.Tests;

[Collection("TransactionsApi")]
public partial class TransactionsController : IClassFixture<TestWebAppFactory<Program>>, IDisposable
{

    private readonly TestWebAppFactory<Program> appFactory;
    private readonly HttpClient appHttpClient;
    private readonly IServiceScope scope;
    private readonly MongoClient mongoClient;
    private readonly DatabaseOptions databaseOptions;
    private readonly string rootUrl = "/transactions";

    public TransactionsController(TestWebAppFactory<Program> factory)
    {
        appFactory = factory;
        appHttpClient = factory.CreateClient();

        scope = appFactory.Services.CreateScope();
        mongoClient = scope.ServiceProvider.GetService<MongoClient>()!;
        databaseOptions = scope.ServiceProvider.GetService<IOptions<DatabaseOptions>>()!.Value;

        mongoClient.DropDatabase(databaseOptions.Name);
    }

    public static IEnumerable<object[]> validCreateTransactionCommandSet = new List<TransactionsCreateCommand[]>
    {
        new []{ new TransactionsCreateCommand { Type = TransactionType.Income,  Amount=10, Date = DateTime.UtcNow, Note = "Test Note" }},
        new []{ new TransactionsCreateCommand { Type = TransactionType.Expense, Amount=10, Date = DateTime.UtcNow, Note = "Test Note" }}

    };

    public static IEnumerable<object[]> invalidCreateTransactionCommandSet = new List<TransactionsCreateCommand[]>
    {
        new [] { new TransactionsCreateCommand { Type = TransactionType.Income, Date = new DateTime(), Note = "Test Note" } },
        new [] { new TransactionsCreateCommand { Type = TransactionType.Income, Amount = 10, Note = "Test Note" } },
        new [] { new TransactionsCreateCommand { Type = TransactionType.Income, Note = "Test Note" } },
        new [] { new TransactionsCreateCommand { Date = new DateTime(), Note = "Test Note" } }
    };

    public async void Dispose()
    {
        await mongoClient.DropDatabaseAsync(databaseOptions.Name);
    }
}
