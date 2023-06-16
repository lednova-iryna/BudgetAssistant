using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Assistants.Budget.BE.Modules.Auth.Domain;

public record IdentityUser(
    Guid Id,
    string UserName,
    IEnumerable<Guid> Roles,
    [property: BsonRepresentation(BsonType.String)] IdentityUserStatus Status,
    Guid CreatedBy,
    DateTime CreatedAt
) { }

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum IdentityUserStatus
{
    Active,
    Inactive
}
