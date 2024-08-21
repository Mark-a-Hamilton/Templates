
var builder = WebApplication.CreateBuilder(args);

#region Log Database
builder.Services.AddDbContext<Domain.Models.LogContext>(options => options.UseSqlServer(
    builder.Configuration.GetValue<string>("Serilog:WriteTo:0:Args:connectionString")));
#endregion
#region Logging
builder.Services.AddExceptionHandler <Domain.Functions.GblExceptionHandler>();

// Configure Serilog using appsettings.json
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();
#endregion

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Configure Telemetry
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
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