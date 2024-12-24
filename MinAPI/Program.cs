var builder = WebApplication.CreateBuilder(args);

#region Configure Serilog Logging for Minimal API
builder.Services.AddExceptionHandler<Domain.Functions.GblExceptionHandler>();

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

Log.Logger = logger; // Add Logger
builder.Host.UseSerilog(logger);
#endregion

#region Configure Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion

#region Configure Validators
// Add custom validators here when needed
#endregion

var app = builder.Build();

#region Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
#endregion

#region Configure Pipeline
app.AddEndpoints();
app.UseSerilogRequestLogging(); // Add Request Logging
app.UseHttpsRedirection();
app.Run();
#endregion
