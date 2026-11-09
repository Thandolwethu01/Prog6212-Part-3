using Microsoft.AspNetCore.Mvc;

namespace RaceDay.MVC.Controllers
{
    public class OrganiserController : Controller
    {
        public IActionResult Index()
        {
            // Only organisers may access this dashboard.
            var role = HttpContext.Session.GetString("UserRole");

            if (role != "Organiser")
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }
    }
}