namespace Domain.Middleware
{
    public class CSPMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CSPMiddleware> _logger;
        private readonly IConfiguration _configuration;

        public CSPMiddleware(RequestDelegate next, ILogger<CSPMiddleware> logger, IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Response.Headers.ContainsKey("Content-Security-Policy") &&
                context.Response.Headers["Content-Security-Policy"].ToString().Contains("block-all-mixed-content"))
            {
                context.Response.Headers.Remove("Content-Security-Policy");
                _logger.LogInformation("Default Content-Security-Policy header removed.");
            }

            if (!context.Response.Headers.ContainsKey("Content-Security-Policy"))
            {
                var connectSrc = _configuration.GetValue<string>("CSP:ConnectSrc");

                context.Response.Headers.Add("Content-Security-Policy",
                    $"default-src 'self'; " +
                    $"img-src 'self' data:; " +
                    $"style-src 'self' 'unsafe-inline' https://cdnjs.cloudflare.com; " +
                    $"style-src-elem 'self' https://cdnjs.cloudflare.com https://cdn.jsdelivr.net https://fonts.googleapis.com; " +
                    $"script-src 'self' 'unsafe-inline' https://cdnjs.cloudflare.com https://code.jquery.com https://cdn.jsdelivr.net; " +
                    $"script-src-elem 'self' 'unsafe-inline' https://cdnjs.cloudflare.com https://code.jquery.com https://cdn.jsdelivr.net; " +
                    $"font-src 'self' https://fonts.googleapis.com https://fonts.gstatic.com https://cdn.jsdelivr.net; " +
                    $"connect-src 'self' {connectSrc};");

                _logger.LogInformation("Custom Content-Security-Policy header added.");
            }
            else
            {
                _logger.LogWarning("Content-Security-Policy header already exists.");
            }

            await _next(context);
        }
    }
}
