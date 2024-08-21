var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

//  Adds All validators that reside in the same assembly as the LogValidator
builder.Services.AddValidatorsFromAssemblyContaining<Domain.Validators.LogValidator>();

#region Configure Logging in Minimal API
// Configure Serilog using appsettings.json
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();
#endregion

//  Add Database Contexts
//  To Add additional databases copy the line below - Remember to change the context & connection string parameters
builder.Services.AddDbContext<Domain.Models.LogContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddEndpoints();

app.UseHttpsRedirection();

app.Run();