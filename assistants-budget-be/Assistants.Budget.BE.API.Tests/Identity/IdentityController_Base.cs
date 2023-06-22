using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Assistants.Budget.BE.API.Tests.Mocks;
using Assistants.Budget.BE.Modules.Auth.Models;
using Assistants.Budget.BE.Modules.Database.Options;
using Assistants.Libs.AspNetCore.Auth.Permissions;
using MongoDB.Driver;

namespace Assistants.Budget.BE.API.Tests.Identity;

[Collection("APITests")]
public partial class IdentityController : IClassFixture<WebAppFactoryMock<Program>>, IDisposable
{
    private readonly WebAppFactoryMock<Program> appFactory;
    private readonly HttpClient appHttpClient;
    private readonly IServiceScope scope;
    private readonly MongoClient mongoClient;
    private readonly DatabaseOptions databaseOptions;
    private readonly string rootUrl = "/identity";

    public IdentityController(WebAppFactoryMock<Program> factory)
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
                new Claim(ClaimTypes.Name, "Test User Identity"),
                new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()),
                new Claim(AssistantsClaimTypes.Permissions, IdentityPermissions.RoleCanCreate),
                new Claim(AssistantsClaimTypes.Permissions, IdentityPermissions.RoleCanDelete),
                new Claim(AssistantsClaimTypes.Permissions, IdentityPermissions.RoleCanEdit),
                new Claim(AssistantsClaimTypes.Permissions, IdentityPermissions.RoleCanRead),
                new Claim(AssistantsClaimTypes.Permissions, IdentityPermissions.UserCanCreate),
                new Claim(AssistantsClaimTypes.Permissions, IdentityPermissions.UserCanDelete),
                new Claim(AssistantsClaimTypes.Permissions, IdentityPermissions.UserCanEdit),
                new Claim(AssistantsClaimTypes.Permissions, IdentityPermissions.UserCanRead)
            }
        );
        appHttpClient.DefaultRequestHeaders.Authorization = new(JwtBearerDefaults.AuthenticationScheme, accessToken);
    }

    public async void Dispose()
    {
        await mongoClient.DropDatabaseAsync(databaseOptions.Name);
    }
}
