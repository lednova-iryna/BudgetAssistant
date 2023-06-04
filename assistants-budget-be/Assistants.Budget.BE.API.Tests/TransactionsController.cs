using System.Net.Http.Json;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Assistants.Budget.BE.Domain;
using Assistants.Budget.BE.BusinessLogic.Transactions.CQRS;
using System.Net;

namespace Assistants.Budget.BE.API.Tests;

[Collection("TransactionsApi")]
public class TransactionsController : IClassFixture<TestWebAppFactory<Program>>
{

    private readonly TestWebAppFactory<Program> _factory;
    private readonly HttpClient _httpClient;

    public TransactionsController(TestWebAppFactory<Program> factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient();
    }

    public static IEnumerable<object[]> validCreateTransactionCommandSet = new List<TransactionsCreateCommand[]>
    {
        new []{ new TransactionsCreateCommand { Type = TransactionType.Income,  Amount=10, Date = DateTime.UtcNow, Note = "Test Note" }},
        new []{ new TransactionsCreateCommand { Type = TransactionType.Expense, Amount=10, Date = DateTime.UtcNow, Note = "Test Note" }}

    };

    public static IEnumerable<object[]> invalidCreateTransactionCommandSet = new List<TransactionsCreateCommand[]>
    {
        new [] { new TransactionsCreateCommand { Type = TransactionType.Income, Date = new DateTime(), Note = "Test Note" } },
        new [] { new TransactionsCreateCommand { Type = TransactionType.Income, Amount=10,  Note = "Test Note" } },
        new [] { new TransactionsCreateCommand { Type = TransactionType.Income, Note = "Test Note" } },
        new [] { new TransactionsCreateCommand { Date = new DateTime(), Note = "Test Note" } }
    };

    [Theory]
    [MemberData(nameof(validCreateTransactionCommandSet))]
    public async Task CreateTransactionAsync(TransactionsCreateCommand command)
    {
        var response = await _httpClient.PutAsJsonAsync("/transactions", command);
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Theory]
    [MemberData(nameof(invalidCreateTransactionCommandSet))]
    public async Task CreateTransactionValidationAsync(TransactionsCreateCommand command)
    {
        var response = await _httpClient.PutAsJsonAsync("/transactions", command);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
