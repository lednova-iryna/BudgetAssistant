namespace Assistants.Budget.BE.Modules.Auth.Domain;

public record IdentityRole(
    Guid Id,
    string Name,
    Guid CreatedBy,
    DateTime CreatedAt,
    IEnumerable<string> Permissions
) { }
