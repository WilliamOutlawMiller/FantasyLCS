using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;

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

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                string cacheKey = $"HomePageData-{User.Identity.Name}";
                HomePage cachedHomePage;

                if (_cache.TryGetValue(cacheKey, out cachedHomePage))
                {
                    if ((int)cachedHomePage.UserLeague.LeagueStatus !> 1)
                        return RedirectToPage("/Home");
                }
                else
                    return RedirectToPage("/Home");

                HomePage = cachedHomePage;
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
