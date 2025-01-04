namespace Domain.Functions;

public class GblExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GblExceptionHandler> _logger;

    public GblExceptionHandler(ILogger<GblExceptionHandler> logger) => _logger = logger;

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
        context.Response.Headers.Append("X-Error-Message", errorMessage);
        await Task.CompletedTask;
        return true;
    }
}