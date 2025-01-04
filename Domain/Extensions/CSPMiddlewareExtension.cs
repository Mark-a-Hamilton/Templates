namespace Domain.Extensions;

public static class CSPMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomCSP(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CSPMiddleware>();
    }
}
