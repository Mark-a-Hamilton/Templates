namespace Domain.Functions;

public class GblExceptionHandler(ILogger<GblExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<GblExceptionHandler> _logger = logger;

    public ValueTask<bool> TryHandleAsync(HttpContext c, 
        Exception e, 
        CancellationToken t)
    {
        _logger.LogError(e, "An exception occurred. HTTP Context: {HttpContext}", c);
        return new ValueTask<bool>(false);
    }
}