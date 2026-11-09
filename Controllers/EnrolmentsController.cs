using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using RaceDay.MVC.Models;
using RaceDay.MVC.Services;

namespace RaceDay.MVC.Controllers
{
    public class EnrolmentsController : Controller
    {
        private readonly RaceDayApiService _api;

        public EnrolmentsController(RaceDayApiService api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("UserRole")
                != "Participant")
            {
                return Forbid();
            }

            var response = await _api.SendAsync(
                HttpMethod.Get,
                "api/registrations/my-enrolments");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return RedirectToAction("Login", "Account");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Unable to load your enrolments.";
                return View(new List<MyEnrolmentViewModel>());
            }

            var enrolments =
                await response.Content
                    .ReadFromJsonAsync<List<MyEnrolmentViewModel>>();

            return View(enrolments ?? new List<MyEnrolmentViewModel>());
        }
    }
}