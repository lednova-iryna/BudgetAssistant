using System.Net;
using System.Net.Http.Json;
using Assistants.Budget.BE.Modules.Transactions.CQRS;

namespace Assistants.Budget.BE.API.Tests.Transactions;

public partial class TransactionsController
{
    [Theory(DisplayName = "Create Transactions")]
    [MemberData(nameof(validCreateTransactionCommandSet))]
    public async Task CreateTransaction(TransactionsCreateCommand command)
    {
        var response = await appHttpClient.PutAsJsonAsync(rootUrl, command);
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Theory(DisplayName = "Create Transactions Validations")]
    [MemberData(nameof(invalidCreateTransactionCommandSet))]
    public async Task CreateTransactionValidation(TransactionsCreateCommand command)
    {
        var response = await appHttpClient.PutAsJsonAsync(rootUrl, command);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
