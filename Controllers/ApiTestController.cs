using Microsoft.AspNetCore.Mvc;

namespace RaceDay.MVC.Controllers
{
    public class ApiTestController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ApiTestController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("RaceDayApi");

                var response = await client.GetAsync("api/events");

                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Successfully connected to the RaceDay API!";
                }
                else
                {
                    ViewBag.Message = "API responded with status: "
                        + response.StatusCode;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Connection failed: " + ex.Message;
            }

            return View();
        }
    }
}