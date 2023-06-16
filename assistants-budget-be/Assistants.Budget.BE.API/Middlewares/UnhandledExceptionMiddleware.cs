namespace Assistants.Budget.BE.API.Middlewares;

public class UnhandledExceptionMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger logger;

    public UnhandledExceptionMiddleware(RequestDelegate next, ILogger<ValidationExceptionMiddleware> logger)
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
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled exception");
            throw;
        }
    }
}
