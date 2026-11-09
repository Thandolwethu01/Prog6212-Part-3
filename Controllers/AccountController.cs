using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using RaceDay.MVC.Models;
using RaceDay.MVC.Services;

namespace RaceDay.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly RaceDayApiService _api;

        public AccountController(RaceDayApiService api)
        {
            _api = api;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var response = await _api.SendAsync(
                    HttpMethod.Post,
                    "api/auth/login",
                    model);

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Invalid email or password. Please try again.");

                    return View(model);
                }

                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

                if (result == null)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "The API returned an unexpected response.");

                    return View(model);
                }

                // Store login information in MVC session.
                HttpContext.Session.SetString("UserRole", result.Role);
                HttpContext.Session.SetString("UserId", result.UserId);

                if (result.Role.Equals(
                    "Organiser",
                    StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction("Index", "Organiser");
                }

                return RedirectToAction("Index", "Participant");
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Unable to connect to the RaceDay API.");

                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _api.SendAsync(
                HttpMethod.Post,
                "api/auth/register",
                model);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] =
                    "Registration successful. Please log in.";

                return RedirectToAction("Login");
            }

            ModelState.AddModelError(
                string.Empty,
                "Registration failed. Check your details and try again.");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _api.SendAsync(
                    HttpMethod.Post,
                    "api/auth/logout");
            }
            finally
            {
                _api.ClearAuthentication();
                HttpContext.Session.Clear();
            }

            return RedirectToAction("Login");
        }

        private class LoginResponse
        {
            public string UserId { get; set; } = string.Empty;
            public string Role { get; set; } = string.Empty;
        }
    }
}