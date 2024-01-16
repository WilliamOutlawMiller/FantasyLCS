using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using FantasyLCS.DataObjects.DataObjects.RequestData;
using Microsoft.Extensions.Caching.Memory;

namespace FantasyLCS.WebApp.Pages;

public class CreateLeagueModel : PageModel
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private readonly string _apiUrl;

    public CreateLeagueModel(HttpClient httpClient, IMemoryCache memoryCache, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _cache = memoryCache;
        _apiUrl = configuration["ApiSettings:BaseUrl"];
    }

    [BindProperty]
    public string LeagueName { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            string username = User.Identity.Name;
            // Prepare the data for creating a team
            CreateLeagueRequest leagueData = new CreateLeagueRequest
            {
                Name = LeagueName,
                LeagueOwner = username
            };

            // Serialize the data to JSON
            var jsonRequest = JsonSerializer.Serialize(leagueData);

            // Create a request content with JSON data
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // Send a POST request to the create team API endpoint
            var leagueResponse = await _httpClient.PostAsync(_apiUrl + "/createleague", content);

            if (leagueResponse.IsSuccessStatusCode)
            {
                var leagueResponseBody = await leagueResponse.Content.ReadAsStringAsync();
                League userLeague = JsonSerializer.Deserialize<League>(leagueResponseBody);

                List<Team> teams = new List<Team>();

                var teamsResponse = await _httpClient.GetAsync(_apiUrl + $"/getteamsbyleagueid/{userLeague.ID}");
                if (teamsResponse.IsSuccessStatusCode)
                {
                    var teamsResponseBody = await teamsResponse.Content.ReadAsStringAsync();
                    teams = JsonSerializer.Deserialize<List<Team>>(teamsResponseBody);
                }

                string cacheKey = $"HomePageData-{username}";
                HomePage cachedHomePage;

                if (_cache.TryGetValue(cacheKey, out cachedHomePage))
                {
                    // Update the UserTeam property with the new team details.
                    cachedHomePage.UserLeague = userLeague;
                    cachedHomePage.LeagueTeams = teams;

                    // Set the updated object back into the cache with the same key.
                    _cache.Set(cacheKey, cachedHomePage);
                }

                return RedirectToPage("/Home");
            }
            else
            {
                // Read the response content as a string
                var errorMessage = await leagueResponse.Content.ReadAsStringAsync();

                // Add the error message to the ModelState
                ModelState.AddModelError(string.Empty, $"Create league failed: {errorMessage}");
                return Page();
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., network errors, API unavailable, etc.)
            ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
            return Page();
        }
    }

    public async Task<IActionResult> OnGetAsync()
    {
        if (User.Identity.IsAuthenticated)
        {
            string cacheKey = $"HomePageData-{User.Identity.Name}";
            HomePage cachedHomePage;

            if (_cache.TryGetValue(cacheKey, out cachedHomePage))
            {
                if (cachedHomePage.UserLeague != null)
                    return RedirectToPage("/Home");
                if (cachedHomePage.UserTeam == null)
                    return RedirectToPage("/Home");
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
