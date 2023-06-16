using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Assistants.Budget.BE.API.Middlewares;
using Assistants.Budget.BE.Modules.Auth;
using Assistants.Budget.BE.Options;
using Assistants.Extensions.Options;
using Assistants.Libs.AspNetCore.Auth;

namespace Assistants.Budget.BE.API.Configurators;

static class AuthConfigurator
{
    public static void AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var generalOptions = OptionsExtensions.LoadOptions<GeneralOptions, GeneralOptions.Validator>(configuration);

        services.AddAuthModule(configuration, generalOptions.Evnironment == "test");
        services.AddSingleton<IAuthorizationMiddlewareResultHandler, AuthMiddlewareResultHandler>();
        services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
        services.AddAuthorization(options =>
        {
            var policyName = "Default";
            options.AddPolicy(
                policyName,
                policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    policy.Requirements.Add(new PermissionRequirement());
                }
            );
            options.DefaultPolicy = options.GetPolicy(policyName)!;
        });
        services.AddHttpContextAccessor();
    }
}
