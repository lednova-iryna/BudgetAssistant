using System.Net;
using System.Net.Http.Json;
using Assistants.Budget.BE.BusinessLogic.Transactions.CQRS;

namespace Assistants.Budget.BE.API.Tests.Transactions;

public partial class TransactionsController
{
    [Theory]
    [MemberData(nameof(validCreateTransactionCommandSet))]
    public async Task CreateTransactionAsync(TransactionsCreateCommand command)
    {
        var response = await appHttpClient.PutAsJsonAsync(rootUrl, command);
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Theory]
    [MemberData(nameof(invalidCreateTransactionCommandSet))]
    public async Task CreateTransactionValidationAsync(TransactionsCreateCommand command)
    {
        var response = await appHttpClient.PutAsJsonAsync(rootUrl, command);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
