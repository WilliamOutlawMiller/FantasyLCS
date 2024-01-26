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

        public string Username { get; set; }

        public string LeagueOwner { get; set; }

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
                    Username = cachedHomePage.UserTeam.OwnerName.ToLower();
                    LeagueOwner = cachedHomePage.UserLeague.Owner.ToLower();
                    var response = await _httpClient.GetAsync(_apiUrl + $"/getdraftplayers/{League.ID}");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        DraftPlayers = JsonSerializer.Deserialize<List<DraftPlayer>>(responseBody);

                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromMinutes(30));

                        _cache.Set(cacheKey, DraftPlayers, cacheEntryOptions);
                    }
                    else
                    {
                        DraftPlayers = new List<DraftPlayer>();
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

        public string GetPositionName(Position position)
        {
            // Assuming you have a method that maps enum values to string names
            return position.ToString().ToUpper(); // Or your custom mapping method
        }
    }
}
