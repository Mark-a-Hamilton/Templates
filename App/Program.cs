var builder = WebApplication.CreateBuilder(args);

#region Configure Serilog Logging
builder.Services.AddSingleton<IExceptionHandler, GblExceptionHandler>();

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

Log.Logger = logger;
builder.Host.UseSerilog(logger);
#endregion Configure Serilog Logging

#region Register Services
builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<ConfigService>();
#endregion Register Services

#region Configure Services
builder.Services.AddHttpClient<ApiService>((serviceProvider, client) =>
{
    var baseAddress = builder.Configuration.GetValue<string>("ApiSettings:BaseAddress");
    if (string.IsNullOrEmpty(baseAddress))
    {
        throw new InvalidOperationException("The BaseAddress setting is missing or empty in the configuration.");
    }

    client.BaseAddress = new Uri(baseAddress);
});

builder.Services.AddRazorPages();

#region Configure Telemetry
builder.Services.AddOpenTelemetry()
    .WithMetrics(opt =>
    {
        opt.AddPrometheusExporter();
        opt.AddMeter("Microsoft.AspNetCore.Hosting", "Microsoft.AspNetCore.Server.Kestrel");
        opt.AddView("request-duration", new ExplicitBucketHistogramConfiguration
        {
            Boundaries = new double[] { 0.005, 0.01, 0.025, 0.05, 0.075, 0.1, 0.25, 0.5, 0.75 }
        });
    });
#endregion Configure Telemetry

#endregion Configure Services

var app = builder.Build();

#region Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
#endregion Configure the HTTP request pipeline.

#region Security Headers
app.UseXContentTypeOptions();
app.UseXXssProtection(options => options.EnabledWithBlockMode());
app.UseCsp(options => options
    .BlockAllMixedContent()
    .StyleSources(s => s.Self().UnsafeInline())
    .FontSources(s => s.Self())
    .FormActions(s => s.Self())
    .FrameAncestors(s => s.Self())
    .ImageSources(s => s.Self())
    .ScriptSources(s => s.Self().UnsafeInline()));
#endregion Security Headers

#region Configure Pipeline
app.UseExceptionHandler(_ => { });
app.MapPrometheusScrapingEndpoint(); // Collect Telemetry
app.UseSerilogRequestLogging(); // Add Request Logging
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, "..", "Domain", "wwwroot")),
    RequestPath = "",
    ServeUnknownFileTypes = true, // This allows serving files with unknown MIME types
    DefaultContentType = "application/octet-stream", // Default MIME type for unknown file types
    OnPrepareResponse = ctx =>
    {
        if (ctx.File.Name.EndsWith(".css"))
        {
            ctx.Context.Response.ContentType = "text/css";
        }
        else if (ctx.File.Name.EndsWith(".js"))
        {
            ctx.Context.Response.ContentType = "application/javascript";
        }
    }
});

app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
#endregion Configure Pipeline

#region Configure Middleware
app.UseCustomCSP();                 // Content Security Protocol Rules
app.UseGlobalExceptionHandler();
#endregion Configure Middleware

app.Run();