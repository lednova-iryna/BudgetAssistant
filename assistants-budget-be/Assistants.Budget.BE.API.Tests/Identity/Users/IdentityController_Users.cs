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
    [Fact(DisplayName = "Create Valid IdentityUser With Status As String")]
    public async Task CreateIdentityUser()
    {
        #region Prepare Data
        var createRoleCommand = new IdentityRoleCreateCommand()
        {
            Name = "TestIdentityRole",
            Permissions = new List<string>
            {
                IdentityPermissions.TransactionCanCreate,
                IdentityPermissions.TransactionCanEdit
            }
        };

        var createRoleResponse = await appHttpClient.PutAsJsonAsync($"{rootUrl}/roles", createRoleCommand);
        var role = await createRoleResponse.Content.ReadFromJsonAsync<IdentityRole>();
        #endregion

        var createUserCommand = new IdentityUserCreateCommand
        {
            UserName = "TestUser",
            Roles = new List<Guid> { role.Id }
        };
        var jsonCommand = JsonConvert.SerializeObject(createUserCommand, new StringEnumConverter());

        var response = await appHttpClient.PutAsync(
            $"{rootUrl}/users",
            new StringContent(jsonCommand, encoding: Encoding.UTF8, mediaType: "application/json")
        );
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var identityUser = await response.Content.ReadFromJsonAsync<IdentityUser>();

        Assert.NotNull(identityUser);
        Assert.Contains(role.Id, identityUser.Roles);
    }

    [Fact(DisplayName = "Get Valid IdentityUser")]
    public async Task GetIdentityUser()
    {
        #region Prepare Data
        var createRoleCommand = new IdentityRoleCreateCommand()
        {
            Name = "TestIdentityRole",
            Permissions = new List<string>
            {
                IdentityPermissions.TransactionCanCreate,
                IdentityPermissions.TransactionCanEdit
            }
        };

        var createRoleResponse = await appHttpClient.PutAsJsonAsync($"{rootUrl}/roles", createRoleCommand);
        var role = await createRoleResponse.Content.ReadFromJsonAsync<IdentityRole>();

        var createUserCommand = new IdentityUserCreateCommand
        {
            UserName = "TestUser",
            Roles = new List<Guid> { role.Id }
        };
        var jsonCommand = JsonConvert.SerializeObject(createUserCommand, new StringEnumConverter());

        var response = await appHttpClient.PutAsync(
            $"{rootUrl}/users",
            new StringContent(jsonCommand, encoding: Encoding.UTF8, mediaType: "application/json")
        );
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        #endregion

        var getUserResponse = await appHttpClient.GetFromJsonAsync<IEnumerable<IdentityUser>>(
            $"{rootUrl}/users?roles={role.Id}&status=Active"
        );
        Assert.NotNull(getUserResponse);
        Assert.Single(getUserResponse);
    }
}
