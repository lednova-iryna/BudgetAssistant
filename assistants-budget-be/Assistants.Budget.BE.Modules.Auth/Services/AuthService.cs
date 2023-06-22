using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Assistants.Budget.BE.Modules.Auth.Options;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Paging;

namespace Assistants.Budget.BE.Modules.Auth;

class AuthService
{
    private readonly IManagementConnection managementConnection;
    private readonly AuthOptions authOptions;
    private readonly IAuthenticationApiClient authenticationApiClient;
    private readonly IDistributedCache cache;
    private ManagementApiClient? managementApiClient;

    public AuthService(
        IManagementConnection managementConnection,
        IOptions<AuthOptions> authOptions,
        IAuthenticationApiClient authenticationApiClient,
        IDistributedCache cache
    )
    {
        this.managementConnection = managementConnection;
        this.authOptions = authOptions.Value;
        this.authenticationApiClient = authenticationApiClient;
        this.cache = cache;

        if (cache == null)
        {
            throw new ArgumentNullException(nameof(cache));
        }
    }

    public async Task<string> GetTokenAsync(
        string clientId,
        string clientSecret,
        string audience,
        CancellationToken cancellationToken
    )
    {
        var cachedToken = await cache.GetStringAsync(clientId, cancellationToken);

        if (!string.IsNullOrWhiteSpace(cachedToken))
        {
            return cachedToken;
        }
        try
        {
            var token = await authenticationApiClient.GetTokenAsync(
                new ClientCredentialsTokenRequest
                {
                    Audience = audience,
                    ClientId = clientId,
                    ClientSecret = clientSecret,
                },
                cancellationToken
            );
            await cache.SetStringAsync(
                clientId,
                token.AccessToken,
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1), }
            );
            managementApiClient = null;
            return token.AccessToken;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<User> CreateUserAsync(UserCreateRequest request, CancellationToken cancellationToken)
    {
        using var apiClient = await GetManagementApiClientAsync(cancellationToken);
        var result = await apiClient.Users.CreateAsync(request, cancellationToken);
        return result;
    }

    public async Task UserAssignRolesAsync(
        string userId,
        AssignRolesRequest request,
        CancellationToken cancellationToken
    )
    {
        using var apiClient = await GetManagementApiClientAsync(cancellationToken);
        await apiClient.Users.AssignRolesAsync(userId, request, cancellationToken);
    }

    public async Task UserUnassignRolesAsync(
        string userId,
        AssignRolesRequest request,
        CancellationToken cancellationToken
    )
    {
        using var apiClient = await GetManagementApiClientAsync(cancellationToken);
        await apiClient.Users.RemoveRolesAsync(userId, request, cancellationToken);
    }

    public async Task<Role> CreateRoleAsync(RoleCreateRequest request, CancellationToken cancellationToken)
    {
        using var apiClient = await GetManagementApiClientAsync(cancellationToken);
        var result = await apiClient.Roles.CreateAsync(request, cancellationToken);
        return result;
    }

    public async Task<IEnumerable<Role>> GetRoles(CancellationToken cancellationToken)
    {
        using var apiClient = await GetManagementApiClientAsync(cancellationToken);
        var result = await apiClient.Roles.GetAllAsync(
            new GetRolesRequest(),
            new PaginationInfo(0, 100, true),
            cancellationToken
        );
        return result.Select(x => x);
    }

    public async Task<Role?> GetRole(string roleId, CancellationToken cancellationToken)
    {
        using var apiClient = await GetManagementApiClientAsync(cancellationToken);
        var result = await apiClient.Roles.GetAsync(roleId, cancellationToken);
        return result;
    }

    public async Task<IEnumerable<Permission>> GetRolePermissions(string roleId, CancellationToken cancellationToken)
    {
        using var apiClient = await GetManagementApiClientAsync(cancellationToken);
        var perPage = 100;
        var pageNo = 0;

        var result = new List<Permission>();
        var response = await apiClient.Roles.GetPermissionsAsync(
            roleId,
            new PaginationInfo(pageNo, perPage, true),
            cancellationToken
        );
        result.AddRange(response.Select(x => x));
        while (response.Paging.Total > result.Count)
        {
            pageNo++;
            response = await apiClient.Roles.GetPermissionsAsync(
                roleId,
                new PaginationInfo(pageNo, perPage, true),
                cancellationToken
            );
            result.AddRange(response.Select(x => x));
        }
        return result;
    }

    public async Task<Role> UpdateRoleAsync(
        string roleId,
        RoleUpdateRequest request,
        CancellationToken cancellationToken
    )
    {
        using var apiClient = await GetManagementApiClientAsync(cancellationToken);
        var result = await apiClient.Roles.UpdateAsync(roleId, request, cancellationToken);
        return result;
    }

    public async Task AssignPermissionsToRoleAsync(
        string roleId,
        AssignPermissionsRequest request,
        CancellationToken cancellationToken
    )
    {
        using var apiClient = await GetManagementApiClientAsync(cancellationToken);
        await apiClient.Roles.AssignPermissionsAsync(roleId, request, cancellationToken);
    }

    private async Task<ManagementApiClient> GetManagementApiClientAsync(CancellationToken cancellationToken)
    {
        var token = await GetTokenAsync(
            authOptions.ManagementApiClientId,
            authOptions.ManagementApiClientSecret,
            authOptions.ManagementApiAudience,
            cancellationToken
        );

        return managementApiClient ??= new ManagementApiClient(
            token,
            new Uri(authOptions.ManagementApiAudience),
            managementConnection
        );
    }
}
