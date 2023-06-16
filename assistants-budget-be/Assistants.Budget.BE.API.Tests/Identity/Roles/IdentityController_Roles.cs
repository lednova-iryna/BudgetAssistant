using System.Net;
using System.Net.Http.Json;
using System.Text;
using Assistants.Budget.BE.Modules.Auth.CQRS;
using Assistants.Budget.BE.Modules.Auth.Domain;
using Assistants.Budget.BE.Modules.Auth.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Assistants.Budget.BE.API.Tests.Identity;

public partial class IdentityController
{
    [Fact(DisplayName = "Create Valid IdentityRole With Permissions As Strings")]
    public async Task CreateValidIdentityRoleWithPermissionsAsStrings()
    {
        var command = new IdentityRoleCreateCommand()
        {
            Name = "TestIdentityRole",
            Permissions = new List<string>
            {
                IdentityPermissions.TransactionCanCreate,
                IdentityPermissions.TransactionCanEdit
            }
        };
        var jsonCommand = JsonConvert.SerializeObject(command, new StringEnumConverter());

        var response = await appHttpClient.PutAsync(
            $"{rootUrl}/roles",
            new StringContent(jsonCommand, encoding: Encoding.UTF8, mediaType: "application/json")
        );

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var identityRole = await response.Content.ReadFromJsonAsync<IdentityRole>();

        Assert.NotNull(identityRole);
        Assert.Contains(IdentityPermissions.TransactionCanCreate, identityRole.Permissions);
        Assert.Contains(IdentityPermissions.TransactionCanEdit, identityRole.Permissions);
    }

    [Fact(DisplayName = "Get IdentityRole By Id")]
    public async Task GetIdentityRoleById()
    {
        var createCommand = new IdentityRoleCreateCommand()
        {
            Name = "TestIdentityRole2",
            Permissions = new List<string>
            {
                IdentityPermissions.TransactionCanCreate,
                IdentityPermissions.TransactionCanEdit
            }
        };

        var createResponse = await appHttpClient.PutAsJsonAsync($"{rootUrl}/roles", createCommand);
        var str = await createResponse.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var identityRole = await createResponse.Content.ReadFromJsonAsync<IdentityRole>();
        Assert.NotNull(identityRole);

        var getIdentityRoleResponse = await appHttpClient.GetFromJsonAsync<IdentityRole>(
            $"{rootUrl}/roles/{identityRole.Id}"
        );
        Assert.NotNull(getIdentityRoleResponse);
        Assert.Equal(identityRole.Id, getIdentityRoleResponse.Id);
    }
}
