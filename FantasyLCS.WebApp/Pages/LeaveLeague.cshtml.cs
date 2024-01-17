using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using FantasyLCS.DataObjects.DataObjects.RequestData;
using Microsoft.Extensions.Caching.Memory;

namespace FantasyLCS.WebApp.Pages;

public class LeaveLeagueModel : PageModel
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private readonly string _apiUrl;

    public LeaveLeagueModel(HttpClient httpClient, IMemoryCache memoryCache, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _cache = memoryCache;
        _apiUrl = configuration["ApiSettings:BaseUrl"];
    }

    public League League { get; private set; }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            string username = User.Identity.Name;

            // Send a POST request to the create team API endpoint
            var response = await _httpClient.PostAsync(_apiUrl + $"/removeuserfromleague/{username}", null);

            if (response.IsSuccessStatusCode)
            {
                string cacheKey = $"HomePageData-{username}";
                HomePage cachedHomePage;

                if (_cache.TryGetValue(cacheKey, out cachedHomePage))
                {
                    // Update the UserTeam property with the new team details.
                    cachedHomePage.UserLeague = null;
                    cachedHomePage.LeagueTeams = null;

                    // Set the updated object back into the cache with the same key.
                    _cache.Set(cacheKey, cachedHomePage);
                }

                return RedirectToPage("/Home");
            }
            else
            {
                // Read the response content as a string
                var errorMessage = await response.Content.ReadAsStringAsync();

                // Add the error message to the ModelState
                ModelState.AddModelError(string.Empty, $"Leave league failed: {errorMessage}");
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
                if (cachedHomePage.UserLeague == null)
                    return RedirectToPage("/Home");
            }
            else
                return RedirectToPage("/Home");

            League = cachedHomePage.UserLeague;
        }
        else
        {
            // If user is not authenticated, redirect to login page
            return RedirectToPage("/Login");
        }

        return Page();
    }
}
