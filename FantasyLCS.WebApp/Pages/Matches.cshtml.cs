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

        public List<LeagueMatch> Matches { get; set; }

        public HomePage HomePage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                string matchCacheKey = $"MatchPageData-{User.Identity.Name}";
                string homeCacheKey = $"HomePageData-{User.Identity.Name}";
                HomePage cachedHomePage;
                List<LeagueMatch> cachedMatches;

                if (_cache.TryGetValue(homeCacheKey, out cachedHomePage))
                {
                    if (cachedHomePage.UserLeague == null)
                        return RedirectToPage("/Home");
                    if ((int)cachedHomePage.UserLeague.LeagueStatus !> 1)
                        return RedirectToPage("/Home");

                    HomePage = cachedHomePage;

                    var response = await _httpClient.GetAsync(_apiUrl + $"/getleaguematches/{cachedHomePage.UserLeague.ID}");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        Matches = JsonSerializer.Deserialize<List<LeagueMatch>>(responseBody);

                        var groupedMatches = Matches.GroupBy(m => m.Week)
                            .ToDictionary(g => g.Key, g => g.ToList());

                        _cache.Set(matchCacheKey, groupedMatches);
                    }
                    else
                    {
                        Matches = new List<LeagueMatch>();
                    }
                }
                else
                    return RedirectToPage("/Home");
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
