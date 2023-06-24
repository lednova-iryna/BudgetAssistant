using System.Net;
using System.Net.Http.Json;
using Assistants.Budget.BE.API.Models;
using Assistants.Budget.BE.Modules.Transactions.CQRS;
using Assistants.Budget.BE.Modules.Transactions.Domain;

namespace Assistants.Budget.BE.API.Tests.Transactions;

public partial class TransactionsController
{
    public static IEnumerable<object[]> invalidTransactionIds = new List<object[]>
    {
        new object[] { Guid.Empty },
        new object[] { null },
        new object[] { "null" },
        new object[] { "str" },
        new object[] { 123 }
    };

    [Theory(DisplayName = "Delete Transactions")]
    [MemberData(nameof(validCreateTransactionCommandSet))]
    public async Task DeleteTransaction(TransactionsCreateCommand command)
    {
        var response = await appHttpClient.PutAsJsonAsync(rootUrl, command);
        var responseString = await response.Content.ReadAsStringAsync();
        var createdTransaction = await response.Content.ReadFromJsonAsync<Transaction>();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(createdTransaction);
        Assert.NotEqual(Guid.Empty, createdTransaction.Id);

        var deleteResponse = await appHttpClient.DeleteAsync($"{rootUrl}/{createdTransaction.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }

    [Theory(DisplayName = "Delete Transactions Validations")]
    [MemberData(nameof(invalidTransactionIds))]
    public async Task DeleteTransactionValidation(object? id)
    {
        var deleteResponse = await appHttpClient.DeleteAsync($"{rootUrl}/{id}");
        Assert.True((int)deleteResponse.StatusCode >= 400 && (int)deleteResponse.StatusCode < 500);

        var responseBodyString = await deleteResponse.Content.ReadAsStringAsync();
    }
}
