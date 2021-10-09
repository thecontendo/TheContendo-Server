using Microsoft.AspNetCore.Mvc;

namespace Contendo.Api.Controllers.Public
{
    public class UsersController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}