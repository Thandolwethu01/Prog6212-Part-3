using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;

namespace RaceDay.MVC.Services
{
    public class RaceDayApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private const string CookieKey = "RaceDayApiCookie";

        public RaceDayApiService(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<HttpResponseMessage> SendAsync(
            HttpMethod method,
            string endpoint,
            object? body = null)
        {
            var context = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException(
                    "No active HTTP context.");

            var client = _httpClientFactory.CreateClient("RaceDayApi");

            using var request = new HttpRequestMessage(method, endpoint);

            // Forward the API authentication cookie, if one exists.
            var savedCookie = context.Session.GetString(CookieKey);

            if (!string.IsNullOrEmpty(savedCookie))
            {
                request.Headers.TryAddWithoutValidation(
                    "Cookie", savedCookie);
            }

            // Attach a JSON request body when supplied.
            if (body != null)
            {
                request.Content = JsonContent.Create(body);
            }

            var response = await client.SendAsync(request);

            // Save authentication cookies returned by the API.
            if (response.Headers.TryGetValues(
                "Set-Cookie", out var cookies))
            {
                var cookiePairs = cookies
                    .Select(cookie => cookie.Split(';')[0])
                    .Where(cookie => !string.IsNullOrWhiteSpace(cookie));

                context.Session.SetString(
                    CookieKey,
                    string.Join("; ", cookiePairs));
            }

            return response;
        }

        public void ClearAuthentication()
        {
            _httpContextAccessor.HttpContext?
                .Session.Remove(CookieKey);
        }
    }
}