using System;
namespace Assistants.Budget.BE.API.Models;

public class ValidationErrorResponse
{
    public string PropertyName { get; set; } = string.Empty;
    public object? PropertyValue { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public int Severity { get; set; } 
}

