namespace Assistants.Libs.AspNetCore.Auth;

/// <summary>
/// Validate "scope" and "permissions" claims
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class PermissionAttribute : Attribute
{
    public PermissionAttribute(params string[] permissions)
    {
        Permissions = permissions;
    }

    public IEnumerable<string> Permissions { get; }
}
