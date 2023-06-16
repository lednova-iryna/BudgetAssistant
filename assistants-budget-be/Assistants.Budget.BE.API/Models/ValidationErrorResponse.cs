namespace Assistants.Budget.BE.API.Models;

public record ValidationErrorResponse(string PropertyName, object? PropertyValue, string ErrorMessage, int Severity);
