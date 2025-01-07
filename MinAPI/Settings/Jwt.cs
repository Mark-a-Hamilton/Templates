namespace API.Settings;

public class Jwt(IConfiguration c)
{
    public string Issuer { get; set; } = c.GetValue<string>("Serilog:Properties:Application") ?? "Default";
    public string Audience { get; set; } = c.GetValue<string>("JwtSettings:Audience") ?? "Default";
    public string SecretKey { get; set; } = c.GetValue<string>("JwtSettings:SecretKey") ?? "Default";
}