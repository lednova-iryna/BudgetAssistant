using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Assistants.Budget.BE.Modules.Auth.CQRS;
using Assistants.Budget.BE.Modules.Auth.Options;
using Assistants.Extensions.Options;
using Auth0.AuthenticationApi;
using Auth0.ManagementApi;

namespace Assistants.Budget.BE.Modules.Auth;

public static class Configurator
{
    public static void AddAuthModule(
        this IServiceCollection services,
        IConfiguration configuration,
        bool isTestEnv = false
    )
    {
        var authOptions = OptionsExtensions.LoadOptions<AuthOptions, AuthOptions.Validator>(configuration, services);

        services
            .AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining<ClientCredentialsTokenQuery>();
            })
            .AddScoped<IAuthenticationApiClient, AuthenticationApiClient>(
                serviceProvider => new AuthenticationApiClient(new Uri(authOptions.Authority))
            )
            .AddSingleton<IManagementConnection, HttpClientManagementConnection>()
            .AddScoped<AuthService>()
            .AddScoped<IdentityService>();

        if (!isTestEnv)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = authOptions.Authority;
                    options.Audience = authOptions.Audience;
                    options.TokenValidationParameters = new() { ValidAudience = authOptions.Audience };
                });
        }
    }
}
