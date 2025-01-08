var builder = WebApplication.CreateBuilder(args);

#region Configure Serilog Logging
builder.Services.AddSingleton<IExceptionHandler, GblExceptionHandler>();

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

Log.Logger = logger;
builder.Host.UseSerilog(logger);
#endregion Configure Serilog Logging

#region Configure Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Identity services
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddIdentityCore<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<DataContext>();

builder.Services.AddControllersWithViews();
#endregion Configure Services

#region Configure Validators
// Add custom validators here when needed
#endregion Configure Validators

#region Configure JWT Authentication
var jwtSettings = new Jwt(builder.Configuration);

builder.Services.AddSingleton(jwtSettings);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
    };
});

builder.Services.AddAuthorization();
#endregion Configure JWT Authentication

var app = builder.Build();

#region Configure Pipeline

#region HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
#endregion HTTP request pipeline

app.UseSerilogRequestLogging(); // Add Request Logging
app.UseHttpsRedirection();
app.UseAuthentication();    // Add Authentication
app.UseAuthorization();     // Add Authorisation 

app.MapControllers();
#endregion Configure Pipeline

app.Run();
