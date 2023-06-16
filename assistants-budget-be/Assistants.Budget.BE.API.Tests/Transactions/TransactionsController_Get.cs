using System.Net;
using System.Net.Http.Json;
using Assistants.Budget.BE.API.Models;
using Assistants.Budget.BE.API.Tests.Helpers;
using Assistants.Budget.BE.Modules.Transactions.CQRS;
using Assistants.Budget.BE.Modules.Transactions.Domain;

namespace Assistants.Budget.BE.API.Tests.Transactions;

public partial class TransactionsController
{
    [Theory(DisplayName = "Get Transaction By Id")]
    [MemberData(nameof(validCreateTransactionCommandSet))]
    public async Task GetTransaction(TransactionsCreateCommand command)
    {
        var createResponse = await appHttpClient.PutAsJsonAsync(rootUrl, command);
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var resstr = await createResponse.Content.ReadAsStringAsync();

        var createResponseObject = await createResponse.Content.ReadFromJsonAsync<Transaction>();
        var getResponse = await appHttpClient.GetFromJsonAsync<Transaction>($"{rootUrl}/{createResponseObject!.Id}");

        Assert.Equal(createResponseObject.Id, getResponse!.Id);
        Assert.Equal(createResponseObject.Date.ToString("U"), getResponse.Date.ToString("U"));
        Assert.Equal(createResponseObject.Amount, getResponse.Amount);
        Assert.Equal(createResponseObject.Type, getResponse.Type);
        Assert.Equal(createResponseObject.Note, getResponse.Note);
    }

    [Fact(DisplayName = "Get Transaction with invalid GUID type")]
    public async Task GetTransactionInvalidId()
    {
        var getResponse = await appHttpClient.GetAsync($"{rootUrl}/wrong-guid");
        Assert.Equal(HttpStatusCode.BadRequest, getResponse.StatusCode);
    }

    [Fact(DisplayName = "Get Transaction with random Id")]
    public async Task GetTransactionRandomId()
    {
        var getResponse = await appHttpClient.GetAsync($"{rootUrl}/{Guid.NewGuid()}");
        Assert.Equal(HttpStatusCode.NoContent, getResponse.StatusCode);

        var createResponseObject = await getResponse.Content.ReadAsStringAsync();
        Assert.Empty(createResponseObject);
    }

    [Fact(DisplayName = "Get Transactions by query")]
    public async Task GetTransactionsValidQuery()
    {
        await appHttpClient.PutAsJsonAsync(
            rootUrl,
            new TransactionsCreateCommand
            {
                Amount = 10,
                Date = DateTime.UtcNow.AddDays(1),
                Type = TransactionType.Income,
                Note = "GetTransactionsValidQueryAsync"
            }
        );
        await appHttpClient.PutAsJsonAsync(
            rootUrl,
            new TransactionsCreateCommand
            {
                Amount = 10,
                Date = DateTime.UtcNow.AddDays(2),
                Type = TransactionType.Income,
                Note = "GetTransactionsValidQueryAsync"
            }
        );
        await appHttpClient.PutAsJsonAsync(
            rootUrl,
            new TransactionsCreateCommand
            {
                Amount = 10,
                Date = DateTime.UtcNow.AddDays(3),
                Type = TransactionType.Income,
                Note = "GetTransactionsValidQueryAsync"
            }
        );
        await appHttpClient.PutAsJsonAsync(
            rootUrl,
            new TransactionsCreateCommand
            {
                Amount = 10,
                Date = DateTime.UtcNow.AddDays(4),
                Type = TransactionType.Income,
                Note = "GetTransactionsValidQueryAsync"
            }
        );

        await appHttpClient.PutAsJsonAsync(
            rootUrl,
            new TransactionsCreateCommand
            {
                Amount = 10,
                Date = DateTime.UtcNow.AddDays(1),
                Type = TransactionType.Expense,
                Note = "GetTransactionsValidQueryAsync"
            }
        );
        await appHttpClient.PutAsJsonAsync(
            rootUrl,
            new TransactionsCreateCommand
            {
                Amount = 10,
                Date = DateTime.UtcNow.AddDays(2),
                Type = TransactionType.Expense,
                Note = "GetTransactionsValidQueryAsync"
            }
        );
        await appHttpClient.PutAsJsonAsync(
            rootUrl,
            new TransactionsCreateCommand
            {
                Amount = 10,
                Date = DateTime.UtcNow.AddDays(3),
                Type = TransactionType.Expense,
                Note = "GetTransactionsValidQueryAsync"
            }
        );
        await appHttpClient.PutAsJsonAsync(
            rootUrl,
            new TransactionsCreateCommand
            {
                Amount = 10,
                Date = DateTime.UtcNow.AddDays(4),
                Type = TransactionType.Expense,
                Note = "GetTransactionsValidQueryAsync"
            }
        );

        var query = UrlHelper.ObjectToQueryString(
            rootUrl,
            new TransactionsQuery
            {
                FromDate = DateTime.UtcNow,
                ToDate = DateTime.UtcNow.AddDays(2.01),
                Type = TransactionType.Income
            }
        );
        var getResponse = await appHttpClient.GetAsync(query);
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var transations = await getResponse.Content.ReadFromJsonAsync<IEnumerable<Transaction>>();
        Assert.Equal(2, transations?.Count());
    }

    [Fact(DisplayName = "Get Transactions from empty collection")]
    public async Task GetTransactionsNoData()
    {
        var query = UrlHelper.ObjectToQueryString(
            rootUrl,
            new TransactionsQuery
            {
                FromDate = DateTime.UtcNow.AddDays(10),
                ToDate = DateTime.UtcNow.AddDays(20),
                Type = TransactionType.Income
            }
        );
        var getResponse = await appHttpClient.GetAsync(query);
        var responseObj = await getResponse.Content.ReadFromJsonAsync<IEnumerable<Transaction>>();

        Assert.Empty(responseObj!);
    }

    [Fact(DisplayName = "Get Transactions by invalid dates range")]
    public async Task GetTransactionsDatesValidation()
    {
        var query = UrlHelper.ObjectToQueryString(
            rootUrl,
            new TransactionsQuery
            {
                FromDate = DateTime.UtcNow.AddDays(10),
                ToDate = DateTime.UtcNow,
                Type = TransactionType.Income
            }
        );
        var response = await appHttpClient.GetAsync(query);
        var responseObject = await response.Content.ReadFromJsonAsync<IEnumerable<ValidationErrorResponse>>();

        Assert.NotNull(responseObject);
        Assert.Single(responseObject);
        Assert.Equal("ToDate", responseObject.Single().PropertyName);
    }
}
