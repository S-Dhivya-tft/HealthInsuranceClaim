using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace HealthInsuranceClaim.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (email == "admin@example.com" && password == "password")
            {
                return RedirectToAction("Index", "Claim");
            }
            ViewBag.Error = "Invalid email or password!";
            return View();
        }
    }
}
