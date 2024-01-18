using FantasyLCS.DataObjects.DataObjects.RequestData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using System.Text;
using FantasyLCS.DataObjects;
using Constants;

namespace FantasyLCS.WebApp.Pages
{
    public class DraftModel : PageModel
    {

        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly string _apiUrl;

        public DraftModel(HttpClient httpClient, IMemoryCache memoryCache, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _cache = memoryCache;
            _apiUrl = configuration["ApiSettings:BaseUrl"];
        }

        public Team CurrentTeam { get; set; }

        public List<Team> Teams { get; set; }

        public League League { get; set; }

        public List<DraftPlayer> DraftPlayers { get; set; }

        public async Task<IActionResult> OnPostStart()
        {
            return RedirectToPage();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                string cacheKey = $"HomePageData-{User.Identity.Name}";
                HomePage cachedHomePage;

                if (_cache.TryGetValue(cacheKey, out cachedHomePage))
                {
                    if (cachedHomePage.UserLeague == null)
                        return RedirectToPage("/Home");
                    if (cachedHomePage.UserLeague.LeagueStatus != LeagueStatus.NotStarted && cachedHomePage.UserLeague.LeagueStatus != LeagueStatus.DraftInProgress)
                        return RedirectToPage("/Home");

                    League = cachedHomePage.UserLeague;
                    Teams = cachedHomePage.LeagueTeams;
                    DraftPlayers = DraftPlayerConstants.DraftPlayers;
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
