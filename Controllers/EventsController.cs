using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using RaceDay.MVC.Models;
using RaceDay.MVC.Services;

namespace RaceDay.MVC.Controllers
{
    public class EventsController : Controller
    {
        private readonly RaceDayApiService _api;

        public EventsController(RaceDayApiService api)
        {
            _api = api;
        }

        // Display all events.
        public async Task<IActionResult> Index(string? type)
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (role != "Organiser" && role != "Participant")
                return RedirectToAction("Login", "Account");

            try
            {
                using var response = await _api.SendAsync(
                    HttpMethod.Get,
                    "api/events");

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    return RedirectToAction("Login", "Account");

                response.EnsureSuccessStatusCode();

                var events = await response.Content
                    .ReadFromJsonAsync<List<EventViewModel>>()
                    ?? new List<EventViewModel>();

                // Apply the selected event-type filter.
                if (!string.IsNullOrWhiteSpace(type))
                {
                    events = events
                        .Where(e => e.EventType.Equals(
                            type,
                            StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                ViewBag.SelectedType = type;
                ViewBag.UserRole = role;

                return View(events);
            }
            catch
            {
                ViewBag.Error = "Unable to load events. Please try again.";
                return View(new List<EventViewModel>());
            }
        }

        // Display the create-event form.
        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("UserRole") != "Organiser")
                return RedirectToAction("Login", "Account");

            return View();
        }

        // Send the new event to the API.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventViewModel model)
        {
            if (HttpContext.Session.GetString("UserRole") != "Organiser")
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
                return View(model);

            if (model.EventDate <= DateTime.Now)
            {
                ModelState.AddModelError(
                    "EventDate",
                    "The event date must be in the future.");

                return View(model);
            }

            try
            {
                using var response = await _api.SendAsync(
                    HttpMethod.Post,
                    "api/events",
                    model);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Event created successfully!";
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(
                    string.Empty,
                    "The API could not create the event. Please check your details.");
            }
            catch
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Unable to connect to the RaceDay API.");
            }

            return View(model);
        }
        // GET: /Events/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (role != "Organiser" && role != "Participant")
                return RedirectToAction("Login", "Account");

            var response = await _api.SendAsync(
                HttpMethod.Get, $"api/events/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Login", "Account");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Could not load the event details.";
                return RedirectToAction(nameof(Index));
            }

            var eventDetails =
                await response.Content.ReadFromJsonAsync<EventDetailsViewModel>();

            if (eventDetails == null)
                return NotFound();

            return View(eventDetails);
        }

        // GET: /Events/AddCategory/5
        public IActionResult AddCategory(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Organiser")
                return Forbid();

            ViewBag.EventID = id;
            return View(new CategoryViewModel());
        }

        // POST: /Events/AddCategory/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(
            int id, CategoryViewModel model)
        {
            if (HttpContext.Session.GetString("UserRole") != "Organiser")
                return Forbid();

            if (!ModelState.IsValid)
            {
                ViewBag.EventID = id;
                return View(model);
            }

            var response = await _api.SendAsync(
                HttpMethod.Post,
                $"api/events/{id}/categories",
                model);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Category added successfully.";
                return RedirectToAction(nameof(Details), new { id });
            }

            ModelState.AddModelError(
                string.Empty,
                "The category could not be added. Check that the event exists and that you manage it.");

            ViewBag.EventID = id;
            return View(model);
        }
    }
}