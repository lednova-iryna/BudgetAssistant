using Microsoft.AspNetCore.Authorization;

namespace Assistants.Libs.AspNetCore.Auth;

public class PermissionRequirement : IAuthorizationRequirement
{
    public PermissionRequirement() { }
}
