namespace Assistants.Budget.BE.API.Middlewares;

public class UnhandledExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public UnhandledExceptionMiddleware(RequestDelegate next, ILogger<ValidationExceptionMiddleware> logger)
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
        catch (Exception ex)
        {
            throw;
        }
    }
}
