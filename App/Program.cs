var builder = WebApplication.CreateBuilder(args);

#region Configure Serilog Logging for Minimal API
builder.Services.AddExceptionHandler<Domain.Functions.GblExceptionHandler>();

//Create Logger from settings from appsettings.json
var logger = new LoggerConfiguration()
.ReadFrom.Configuration(builder.Configuration)
.CreateLogger();

Log.Logger = logger; //Add Logger
builder.Host.UseSerilog(logger);
#endregion

#region Configure services
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
#endregion

#region Configure Telemetry
builder.Services.AddOpenTelemetry()
    .WithMetrics(opt =>
    {
        opt.AddPrometheusExporter();
        opt.AddMeter("Microsoft.AspNetCore.Hosting", 
            "Microsoft.AspNetCore.Server.Kestrel");
        opt.AddView("request-duration", new ExplicitBucketHistogramConfiguration
        {
            Boundaries = [0.005,
            0.01,
            0.025,
            0.05,
            0.075,
            0.1,
            0.25,
            0.5,
            0.75]
        });
    });
#endregion

var app = builder.Build();

#region Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
#endregion

#region Configure Pipeline
app.UseExceptionHandler(_ => { });
app.MapPrometheusScrapingEndpoint(); // Collect Telemetry
app.UseSerilogRequestLogging(); // Add Request Logging
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
#endregion