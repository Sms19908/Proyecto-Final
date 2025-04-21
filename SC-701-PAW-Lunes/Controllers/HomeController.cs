using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SC_701_PAW_Lunes.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewData["UserMessage"] = $"Bienvenido, {User.Identity.Name}";
            }
            else
            {
                ViewData["UserMessage"] = "Bienvenido, por favor inicia sesión para acceder a más funciones.";
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogError("Se ha producido un error inesperado.");
            return View();
        }
    }
}
