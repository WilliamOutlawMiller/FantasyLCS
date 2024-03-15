using Constants;
using FantasyLCS.DataObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace FantasyLCS.WebApp.Pages
{
    public class MatchesModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly string _apiUrl;

        public MatchesModel(HttpClient httpClient, IMemoryCache memoryCache, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _cache = memoryCache;
            _apiUrl = configuration["ApiSettings:BaseUrl"];
        }

        public HomePage HomePage { get; set; }

        public MatchesPage MatchesPage { get; set; } = new MatchesPage();

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                var username = User.Identity.Name;
                string matchCacheKey = $"MatchPageData-{User.Identity.Name}";
                string homeCacheKey = $"HomePageData-{User.Identity.Name}";
                HomePage cachedHomePage;
                MatchesPage cachedMatchesPage;

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(30));

                // Get HomePage
                if (_cache.TryGetValue(homeCacheKey, out cachedHomePage))
                    HomePage = cachedHomePage;
                else
                    return RedirectToPage("/Home");

                if (HomePage.UserLeague == null)
                        return RedirectToPage("/Home");
                if ((int)HomePage.UserLeague.LeagueStatus < 1)
                        return RedirectToPage("/Home");

                // Get LeagueMatches
                if (_cache.TryGetValue(matchCacheKey, out cachedMatchesPage))
                    MatchesPage = cachedMatchesPage;
                else
                {
                    var response = await _httpClient.GetAsync(_apiUrl + $"/getmatchespage/{username}");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        MatchesPage = JsonSerializer.Deserialize<MatchesPage>(responseBody);
                    }
                }

                _cache.Set(matchCacheKey, MatchesPage, cacheEntryOptions);
            }
            else
            {
                // If user is not authenticated, redirect to login page
                return RedirectToPage("/Login");
            }

            return Page();
        }
    }
}
