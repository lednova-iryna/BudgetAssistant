using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Assistants.Budget.BE.Modules.Transactions.Domain;

public record Transaction(
    Guid Id,
    DateTime Date,
    double Amount,
    [property: BsonRepresentation(BsonType.String)] TransactionType Type,
    Guid CreatedBy,
    DateTime CreatedAt,
    string? Note
) { }

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TransactionType
{
    Income,
    Expense
}
