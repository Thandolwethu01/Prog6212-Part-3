using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using RaceDay.MVC.Models;
using RaceDay.MVC.Services;

namespace RaceDay.MVC.Controllers
{
    public class ResultsController : Controller
    {
        private readonly RaceDayApiService _api;

        public ResultsController(RaceDayApiService api)
        {
            _api = api;
        }

        public async Task<IActionResult> MyResults()
        {
            if (HttpContext.Session.GetString("UserRole")
                != "Participant")
            {
                return Forbid();
            }

            var response = await _api.SendAsync(
                HttpMethod.Get,
                "api/results/my-results");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return RedirectToAction("Login", "Account");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Unable to load your race results.";
                return View(new List<MyResultViewModel>());
            }

            var results = await response.Content
                .ReadFromJsonAsync<List<MyResultViewModel>>();

            return View(results ?? new List<MyResultViewModel>());
        }
    }
}