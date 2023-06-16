using System.Net;
using Assistants.Budget.BE.API.Models;
using FluentValidation;

namespace Assistants.Budget.BE.API.Middlewares;

public class ValidationExceptionMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger logger;

    public ValidationExceptionMiddleware(RequestDelegate next, ILogger<ValidationExceptionMiddleware> logger)
    {
        this.logger = logger;
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (ValidationException ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, ValidationException exception)
    {
        logger.LogError(exception, "Validation exception");
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        await context.Response.WriteAsJsonAsync(
            exception.Errors.Select(
                x => new ValidationErrorResponse(x.PropertyName, x.AttemptedValue, x.ErrorMessage, (int)x.Severity)
            )
        );
    }
}
