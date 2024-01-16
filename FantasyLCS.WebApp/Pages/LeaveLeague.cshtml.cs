using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using FantasyLCS.DataObjects.DataObjects.RequestData;

namespace FantasyLCS.WebApp.Pages;

public class LeaveLeagueModel : PageModel
{
    private readonly HttpClient _httpClient;

    public LeaveLeagueModel(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public League League { get; private set; }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            string username = User.Identity.Name;

            // Send a POST request to the create team API endpoint
            var response = await _httpClient.PostAsync($"https://api.fantasy-lcs.com/removeuserfromleague/{username}", null);

            if (response.IsSuccessStatusCode)
            {
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
            var username = User.Identity.Name;

            var response = await _httpClient.GetAsync($"https://api.fantasy-lcs.com/getleaguebyusername/{username}");

            if (response.IsSuccessStatusCode)
            {
                var leagueJson = await response.Content.ReadAsStringAsync();
                var league = JsonSerializer.Deserialize<League>(leagueJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                League = league;
            }
            else
            {
                return RedirectToPage("/Home");
            }
        }
        else
        {
            // If user is not authenticated, redirect to login page
            return RedirectToPage("/Login");
        }

        return Page();
    }
}
