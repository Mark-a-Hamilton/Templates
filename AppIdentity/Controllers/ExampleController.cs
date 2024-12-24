namespace AppIdentity.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExampleController : Controller
{
    #region Initialise Controller
    private readonly ApiService _apiService;
    private readonly ILogger<ExampleController> _logger;

    #pragma warning disable IDE0290
    public ExampleController(ApiService apiService, ILogger<ExampleController> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }
    #pragma warning restore IDE0290
    #endregion

    #region Test Screen Endpoint calls
    [Route("test")]
    public async Task<IActionResult> Test()
    {
        var data = await _apiService.GetDataAsync("api/example/eg");
        ViewData["ApiData"] = data; // Pass the data to the view using ViewData return View("Test");
        return View("Test");
    }
    #endregion

    #region Stack Trace on Error !!!!
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [Route("example/error")]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    #endregion
}