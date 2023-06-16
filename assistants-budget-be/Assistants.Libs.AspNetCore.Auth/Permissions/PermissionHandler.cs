using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Assistants.Libs.AspNetCore.Auth;

public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement
    )
    {
        var targets = GetTargetPermissions(context);
        if (targets?.Any() == true)
        {
            var userPermissions = context.User.FindAll(c => c.Type == "permissions").Select(x => x.Value).ToHashSet();
            if (userPermissions.IsSupersetOf(targets))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            var userScopes = context.User.FindFirst(c => c.Type == "scope")?.Value.Split(" ").ToHashSet();
            if (userScopes != null && userScopes.IsSupersetOf(targets))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
        }
        else
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }

    private static HashSet<string>? GetTargetPermissions(AuthorizationHandlerContext context)
    {
        var routeEndpoint = context.Resource as DefaultHttpContext;
        var descriptor = routeEndpoint?.GetEndpoint()?.Metadata.OfType<ControllerActionDescriptor>().SingleOrDefault();
        if (descriptor != null)
        {
            var scopeAttribute =
                (PermissionAttribute?)descriptor.MethodInfo.GetCustomAttribute(typeof(PermissionAttribute))
                ?? (PermissionAttribute?)descriptor.ControllerTypeInfo.GetCustomAttribute(typeof(PermissionAttribute));

            return scopeAttribute?.Permissions.ToHashSet();
        }

        return null;
    }
}
