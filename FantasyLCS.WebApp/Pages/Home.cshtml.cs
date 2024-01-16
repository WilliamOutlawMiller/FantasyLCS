using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using FantasyLCS.DataObjects;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace FantasyLCS.WebApp.Pages;

public class HomeModel : PageModel
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private readonly string _apiUrl;

    public HomePage HomePage { get; set; }

    public HomeModel(HttpClient httpClient, IMemoryCache memoryCache, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _cache = memoryCache;
        _apiUrl = configuration["ApiSettings:BaseUrl"];
    }

    public async Task<IActionResult> OnGet()
    {
        // todo: we can update the API with an endpoint called LoadHomePage that returns all the info we need in one request.

        if (User.Identity.IsAuthenticated)
        {
            var username = User.Identity.Name;
            string cacheKey = $"HomePageData-{username}";
            HomePage cachedHomePage;

            if (!_cache.TryGetValue(cacheKey, out cachedHomePage))
            {
                var response = await _httpClient.GetAsync(_apiUrl + $"/gethomepage/{username}");

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    cachedHomePage = JsonSerializer.Deserialize<HomePage>(responseBody);

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(30));

                    // Save data in cache
                    _cache.Set(cacheKey, cachedHomePage, cacheEntryOptions);
                }
            }

            HomePage = cachedHomePage;

            return Page();
        }
        else
        {
            // If user is not authenticated, redirect to login page
            return RedirectToPage("/Login");
        }
    }
}
