namespace AppIdentity.Controllers
{
    public class HomeController(ILogger<HomeController> logger) : Controller
    {
        #region Initialise Controller
        private readonly ILogger<HomeController> _logger = logger;
        #endregion

        #region Home Srceen Endpoint calls
        public IActionResult Index() { return View(); }

        public IActionResult Privacy() { return View(); }
        #endregion

        #region Stack Trace on Error !!!!
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Toast() { return View(); }
        #endregion
    }
}