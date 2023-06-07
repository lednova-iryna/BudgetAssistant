using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Assistants.Budget.BE.API.Tests.Mocks;

public static class AuthModuleMock
{
    public static string Issuer { get; } = Guid.NewGuid().ToString();
    public static string Audience { get; } = Guid.NewGuid().ToString();
    public static SecurityKey SecurityKey { get; }
    public static SigningCredentials SigningCredentials { get; }

    private static readonly JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
    private static readonly RandomNumberGenerator generator = RandomNumberGenerator.Create();
    private static readonly byte[] key = new byte[32];

    static AuthModuleMock()
    {
        generator.GetBytes(key);
        SecurityKey = new SymmetricSecurityKey(key) { KeyId = Guid.NewGuid().ToString() };
        SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
    }

    public static string GenerateJwtToken()
    {
        return tokenHandler.WriteToken(
            new JwtSecurityToken(Issuer, Audience, null, null, DateTime.UtcNow.AddMinutes(10), SigningCredentials)
        );
    }

    public static void AddAuth0ModuleMock(this IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.PostConfigure<JwtBearerOptions>(
                JwtBearerDefaults.AuthenticationScheme,
                options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = AuthModuleMock.SecurityKey,
                        ValidIssuer = AuthModuleMock.Issuer,
                        ValidAudience = AuthModuleMock.Audience
                    };
                }
            );
            services
                .AddAuthentication(defaultScheme: JwtBearerHandlerMock.TestSchema)
                .AddScheme<JwtBearerOptions, JwtBearerHandlerMock>(JwtBearerHandlerMock.TestSchema, options => { });
        });
    }
}

public class JwtBearerHandlerMock : JwtBearerHandler
{
    public static readonly string TestSchema = "TestSchema";

    public JwtBearerHandlerMock(
        IOptionsMonitor<JwtBearerOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock
    )
        : base(options, logger, encoder, clock) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[] { new Claim(ClaimTypes.Name, "Test user") };
        var identity = new ClaimsIdentity(claims, "TestIdentity");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, TestSchema);

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}
