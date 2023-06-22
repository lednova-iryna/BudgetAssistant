namespace Assistants.Budget.BE.Modules.Auth.Domain;

public record IdentityRole(string Id, string Name, string? Description, IEnumerable<string>? Permissions = null) { }
