using Microsoft.AspNetCore.Mvc;

namespace IdentityServer4.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
