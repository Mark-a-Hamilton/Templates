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

#region Register Services
builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<ConfigService>();
#endregion

#region Configure services
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("Default") ?? throw new InvalidOperationException("Connection string 'Default' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
 
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add HttpClient with BaseAddress from configuration
builder.Services.AddHttpClient<ApiService>((serviceProvider, client) =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiSettings:BaseAddress"));
});

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
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
#endregion

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
#endregion

#region Configure Pipeline
app.UseExceptionHandler(_ => { });
app.MapPrometheusScrapingEndpoint(); // Collect Telemetry
app.UseSerilogRequestLogging(); // Add Request Logging
app.UseHttpsRedirection();
app.UseStaticFiles();

// Configure the application to use static files from the Domain project
app.UseStaticFiles(new StaticFileOptions { 
    FileProvider = new PhysicalFileProvider( Path.Combine(app.Environment.ContentRootPath, "..", "Domain", "wwwroot")), 
    RequestPath = "/wwwroot" 
});

app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
#endregion