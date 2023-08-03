using Microsoft.AspNetCore.Mvc;

namespace Portfolio.Controllers
{
    public class AccountController : Controller
    {
        [Route("login")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
