using Microsoft.AspNetCore.Mvc;

namespace Contendo.Controllers
{
    public class HomeController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}