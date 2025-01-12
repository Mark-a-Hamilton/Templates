namespace Domain.Middleware;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IExceptionHandler _exceptionHandler;

    public ExceptionHandlerMiddleware(RequestDelegate next, IExceptionHandler exceptionHandler)
    {
        _next = next;
        _exceptionHandler = exceptionHandler;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);

            // Handle status code pages for web page not found
            if (context.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                throw new PageNotFoundException("Page not found");
            }
        }
        catch (Exception ex)
        {
            await _exceptionHandler.TryHandleAsync(context, ex, CancellationToken.None);
        }
    }
}
