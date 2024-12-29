namespace Domain.Functions;

public class GblExceptionHandler(ILogger<GblExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<GblExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext context, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An exception occurred. HTTP Context: {HttpContext}", context);
        var (statusCode, errorMessage) = exception switch
        {
            ArgumentNullException or ArgumentException => (StatusCodes.Status400BadRequest, "Bad request"),
            InvalidOperationException => (StatusCodes.Status409Conflict, "Invalid operation"),
            FileNotFoundException => (StatusCodes.Status404NotFound, "File not found"),
            DirectoryNotFoundException => (StatusCodes.Status404NotFound, "Directory not found"),
            UnauthorizedAccessException => (StatusCodes.Status403Forbidden, "Access denied"),
            NotImplementedException => (StatusCodes.Status501NotImplemented, "Not implemented"),
            TimeoutException => (StatusCodes.Status408RequestTimeout, "Request timed out"),
            SqlException => (StatusCodes.Status500InternalServerError, "Database error"),
            HttpRequestException => (StatusCodes.Status503ServiceUnavailable, "Service unavailable"),
            TaskCanceledException => (StatusCodes.Status499ClientClosedRequest, "Request cancelled"),
            AggregateException => (StatusCodes.Status500InternalServerError, "Multiple errors occurred"),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };

        context.Response.StatusCode = statusCode;
        context.Response.Headers.Add("X-Error-Message", errorMessage);
        context.Response.Redirect("/Home/Toast");
        return true;
    }
}

/*
public class GblExceptionHandler(ILogger<GblExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<GblExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An exception occurred. HTTP Context: {HttpContext}", context);
        context.Response.ContentType = "application/json";
        var (statusCode, errorMessage) = exception switch
        {
            // If an exveption is unhandled add a general exception but be more specific when the exception is thrown 
             
            ArgumentNullException or ArgumentException => (StatusCodes.Status400BadRequest, "Bad request"),
            InvalidOperationException => (StatusCodes.Status409Conflict, "Invalid operation"),
            FileNotFoundException => (StatusCodes.Status404NotFound, "File not found"),
            DirectoryNotFoundException => (StatusCodes.Status404NotFound, "Directory not found"),
            UnauthorizedAccessException => (StatusCodes.Status403Forbidden, "Access denied"),
            NotImplementedException => (StatusCodes.Status501NotImplemented, "Not implemented"),
            TimeoutException => (StatusCodes.Status408RequestTimeout, "Request timed out"),
            SqlException => (StatusCodes.Status500InternalServerError, "Database error"),
            HttpRequestException => (StatusCodes.Status503ServiceUnavailable, "Service unavailable"),
            TaskCanceledException => (StatusCodes.Status499ClientClosedRequest, "Request cancelled"),
            AggregateException => (StatusCodes.Status500InternalServerError, "Multiple errors occurred"),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };

        context.Response.StatusCode = statusCode;
        var result = exception is AggregateException aggregateException
            ? System.Text.Json.JsonSerializer.Serialize(new { error = errorMessage, details = aggregateException.InnerExceptions.Select(e => e.Message) })
            : System.Text.Json.JsonSerializer.Serialize(new { error = errorMessage });
        await context.Response.WriteAsync(result, cancellationToken);
        return true;
    }
}
*/