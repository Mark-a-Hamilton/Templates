namespace App.Controllers;

public class HomeController : Controller
{
    #region Initialise Controller
    private readonly ApiService _apiService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ApiService apiService, ILogger<HomeController> logger) 
    {
        _apiService = apiService;
        _logger = logger; 
    }
    #endregion

    #region Home Srceen Endpoint calls
    public IActionResult Index() { return View(); }

    public IActionResult Test()
    {
        //var data = await _apiService.GetDataAsync<string>("api/example/eg"); 
        //return View("Test", data);
        return View();
    }
    public IActionResult Privacy() { return View(); }
    #endregion

    #region Stack Trace on Error !!!!
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    { 
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }); 
    }
    #endregion
}