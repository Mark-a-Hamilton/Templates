namespace Domain.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ApiService> _logger;

    public ApiService(HttpClient httpClient, ILogger<ApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<string> GetDataAsync(string endpoint)
    {
        var fullUrl = new Uri(_httpClient.BaseAddress, endpoint);
        _logger.LogInformation($"Requesting URL: {fullUrl}");   // Log the endpoint to ensure it is a full URL

        var response = await _httpClient.GetAsync(fullUrl);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(); // Return the plain text content directly
    }
}