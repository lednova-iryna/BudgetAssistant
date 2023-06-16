using System.Net;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization.Policy;
using Assistants.Libs.AspNetCore.Auth;

namespace Assistants.Budget.BE.API.Middlewares;

public class AuthMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();
    private readonly ILogger<AuthMiddlewareResultHandler> logger;

    public AuthMiddlewareResultHandler(ILogger<AuthMiddlewareResultHandler> logger)
    {
        this.logger = logger;
    }

    public async Task HandleAsync(
        RequestDelegate next,
        HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult
    )
    {
        if (authorizeResult.Forbidden)
        {
            StringBuilder errorMessage = new StringBuilder("Common error");

            if (authorizeResult.AuthorizationFailure!.FailedRequirements.OfType<ClaimsAuthorizationRequirement>().Any())
            {
                errorMessage.Clear().Append("Incorrect claims");
            }
            else if (authorizeResult.AuthorizationFailure!.FailedRequirements.OfType<PermissionRequirement>().Any())
            {
                errorMessage.Clear().Append("Insufficient permissions to perform this action");
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync(errorMessage.ToString());
            return;
        }

        // Fall back to the default implementation.
        await defaultHandler.HandleAsync(next, context, policy, authorizeResult);
    }
}
