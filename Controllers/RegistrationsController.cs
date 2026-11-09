using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Models;
using RaceDay.MVC.Services;

namespace RaceDay.MVC.Controllers
{
    public class RegistrationsController : Controller
    {
        private readonly RaceDayApiService _api;

        public RegistrationsController(RaceDayApiService api)
        {
            _api = api;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enrol(
            int eventId, EnrolmentViewModel model)
        {
            if (HttpContext.Session.GetString("UserRole") != "Participant")
                return Forbid();

            if (!ModelState.IsValid)
                return RedirectToAction("Details", "Events",
                    new { id = eventId });

            var response = await _api.SendAsync(
                HttpMethod.Post,
                "api/registrations",
                new { CategoryID = model.CategoryID });

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "You have successfully enrolled in this category.";
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();

                TempData["Error"] =
                    "Enrolment failed. You may already be enrolled, or the event may no longer accept registrations.";
            }

            return RedirectToAction("Details", "Events",
                new { id = eventId });
        }
    }
}