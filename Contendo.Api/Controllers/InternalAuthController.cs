using Microsoft.AspNetCore.Mvc;

namespace Contendo.Api.Controllers
{
    public class InternalAuthController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}