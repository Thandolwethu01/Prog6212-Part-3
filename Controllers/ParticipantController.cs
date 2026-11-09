using Microsoft.AspNetCore.Mvc;

namespace RaceDay.MVC.Controllers
{
    public class ParticipantController : Controller
    {
        public IActionResult Index()
        {
            // Only participants may access this dashboard.
            var role = HttpContext.Session.GetString("UserRole");

            if (role != "Participant")
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }
    }
}