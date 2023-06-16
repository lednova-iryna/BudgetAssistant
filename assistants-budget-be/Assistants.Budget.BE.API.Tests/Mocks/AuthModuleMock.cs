using System.IdentityModel.Tokens.Jwt;
using System.Net;
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
using Moq;
using Moq.Protected;
using Newtonsoft.Json;

namespace Assistants.Budget.BE.API.Tests.Mocks;

static class AuthModuleMock
{
    internal static string Issuer { get; } = Guid.NewGuid().ToString();
    internal static string Audience { get; } = Guid.NewGuid().ToString();
    internal static JwtHelper JwtHelper { get; } = new JwtHelper();

    public static string GenerateJwtToken(IEnumerable<Claim> claims)
    {
        return AuthModuleMock.JwtHelper.IssueJwtToken(Issuer, Audience, claims);
    }

    public static void AddAuthModuleMock(this IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // PostConfigure<JwtBearerOptions> doesn't work. didn't find answer why.
            // Need to make sure to avoid AddAuthentication in main applicaiton during tests.

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddScheme<JwtBearerOptions, JwtBearerHandlerMock>(
                    JwtBearerDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            SignatureValidator = (token, parameters) =>
                            {
                                return new JwtSecurityToken(token);
                            }
                        };
                        options.Audience = Audience;
                        options.Authority = "https://Auth__Domain";
                        options.BackchannelHttpHandler = new MockBackchannel();
                        options.MetadataAddress = "https://Auth__Domain/.well-known/openid-configuration";
                    }
                );
        });
    }

    private static HttpClient GenerateHttpClientMock(string authority)
    {
        var mockMessageHandler = new Mock<HttpMessageHandler>();
        mockMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(
                    req =>
                        req.Method == HttpMethod.Get
                        && req.RequestUri == new Uri($"{authority}/.well-known/openid-configuration")
                ),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(
                new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JwtHelper.JwksString)
                }
            );

        return new HttpClient(mockMessageHandler.Object);
    }

    private class MockBackchannel : HttpMessageHandler
    {
        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri.AbsoluteUri.Contains(".well-known/openid-configuration"))
            {
                return new HttpResponseMessage
                {
                    Content = new StringContent(AuthModuleMock.JwtHelper.JwksString),
                    StatusCode = HttpStatusCode.OK
                };
            }
            return base.Send(request, cancellationToken);
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken
        )
        {
            if (request.RequestUri.AbsoluteUri.Contains(".well-known/openid-configuration"))
            {
                return new HttpResponseMessage
                {
                    Content = new StringContent(AuthModuleMock.JwtHelper.JwksString),
                    StatusCode = HttpStatusCode.OK
                };
            }

            throw new NotImplementedException();
        }
    }

    private class JwtBearerHandlerMock : JwtBearerHandler
    {
        public JwtBearerHandlerMock(
            IOptionsMonitor<JwtBearerOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
        )
            : base(options, logger, encoder, clock) { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var jwtTokenString = Context.Request.Headers.Authorization.ToString().Split(" ").Last();
            var jwtToken = new JwtSecurityToken(jwtTokenString);

            var identity = new ClaimsIdentity(jwtToken.Claims, "TestIdentity");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, JwtBearerDefaults.AuthenticationScheme);

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
}

class JwtHelper
{
    public readonly RSA rsa;
    public RsaSecurityKey SecurityKey { get; }
    public readonly JsonWebKey jwk;
    public string JwksString { get; private set; }

    public JwtHelper()
    {
        this.rsa = RSA.Create();
        this.SecurityKey = new RsaSecurityKey(rsa) { KeyId = Guid.NewGuid().ToString() };
        this.jwk = JsonWebKeyConverter.ConvertFromRSASecurityKey(SecurityKey);
        Dictionary<string, IList<JsonWebKey>> jwksDict =
            new()
            {
                {
                    "keys",
                    new List<JsonWebKey> { jwk }
                }
            };
        this.JwksString = JsonConvert.SerializeObject(jwksDict);
    }

    public string IssueJwtToken(string issuer, string audience, IEnumerable<Claim> claims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(15),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.RsaSha256)
        };

        var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
