using System;
using System.Net;
using Assistants.Budget.BE.API.Models;
using FluentValidation;

namespace Assistants.Budget.BE.API.Middlewares;

public class ValidationExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    public ValidationExceptionMiddleware(RequestDelegate next, ILogger<ValidationExceptionMiddleware> logger)
    {
        _logger = logger;
        _next = next;
    }
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (ValidationException ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, ValidationException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        await context.Response.WriteAsJsonAsync(exception.Errors.Select(x=> new ValidationErrorResponse
        {
            ErrorMessage = x.ErrorMessage,
            PropertyName = x.PropertyName,
            Severity = (int)x.Severity,
            PropertyValue = x.AttemptedValue
        }));
    }
}

