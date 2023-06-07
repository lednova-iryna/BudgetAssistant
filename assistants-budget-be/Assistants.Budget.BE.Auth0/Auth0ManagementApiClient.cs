using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Assistants.Budget.BE.Options;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.ManagementApi;

namespace Assistants.Budget.BE.Auth0;

public class Auth0ManagementApiClient
{
    private const string TokenCacheKey = "ManagementApiTokenKey";

    private readonly IManagementConnection managementConnection;
    private readonly AuthOptions authOptions;
    private readonly IAuthenticationApiClient authenticationApiClient;
    private readonly IMemoryCache memoryCache;

    public Auth0ManagementApiClient(
        IManagementConnection managementConnection,
        IOptions<AuthOptions> authOptions,
        IAuthenticationApiClient authenticationApiClient,
        IMemoryCache memoryCache
    )
    {
        this.managementConnection = managementConnection;
        this.authOptions = authOptions.Value;
        this.authenticationApiClient = authenticationApiClient;
        this.memoryCache = memoryCache;
    }

    public async Task<string> GetTokenAsync(string clientId, string clientSecret, CancellationToken cancellationToken)
    {
        if (memoryCache.TryGetValue<string>(TokenCacheKey, out var cachedToken))
        {
            return cachedToken;
        }
        try
        {
            var token = await authenticationApiClient.GetTokenAsync(
                new ClientCredentialsTokenRequest
                {
                    Audience = authOptions.Audience,
                    ClientId = authOptions.ClientId ?? clientId,
                    ClientSecret = authOptions.ClientSecret ?? clientSecret,
                },
                cancellationToken
            );
            memoryCache.Set(TokenCacheKey, token.AccessToken, TimeSpan.FromSeconds(token.ExpiresIn - 1));

            return token.AccessToken;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
