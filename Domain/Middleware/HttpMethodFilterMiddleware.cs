namespace Domain.Middleware;

public class HttpMethodFilterMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var allowedMethods = new Dictionary<string, string[]>
    {
        { "/api/example/error", new[] { "GET" } },
        { "/api/example/test", new[] { "GET" } }
    };

        if (allowedMethods.TryGetValue(context.Request.Path, out var methods) &&
            !methods.Contains(context.Request.Method, StringComparer.OrdinalIgnoreCase))
        {
            context.Response.StatusCode = StatusCodes.Status405MethodNotAllowed;
            await context.Response.WriteAsync("Method Not Allowed");
            return;
        }

        await _next(context);
    }
}