namespace Domain.Extensions;

public static class ExceptionHandlerExtension
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<Middleware.ExceptionHandlerMiddleware>();
    }
}
