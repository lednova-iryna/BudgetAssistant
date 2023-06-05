using System;
using Assistants.Budget.BE.Options;
using Assistants.Extensions.Options;
using Auth0.AuthenticationApi;
using Auth0.ManagementApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Assistants.Budget.BE.Auth0;

public static class Configurator
{
    public static void AddAuth0Module(this IServiceCollection services, IConfiguration configuration)
    {
        var authOptions = OptionsExtensions.LoadOptions<AuthOptions, AuthOptions.Validator>(configuration, services);

        services.AddScoped<IAuthenticationApiClient, AuthenticationApiClient>(serviceProvider => new AuthenticationApiClient(new Uri(authOptions.Domain)));
        services.AddSingleton<IManagementConnection, HttpClientManagementConnection>();
        services.AddScoped<Auth0ManagementApiClient>();
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = authOptions.Domain;
                options.Audience = authOptions.Audience;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidAudience = authOptions.Audience
                };
            });
    }
}

