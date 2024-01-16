using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using FantasyLCS.DataObjects.DataObjects.RequestData;

namespace FantasyLCS.WebApp.Pages;

public class CreateLeagueModel : PageModel
{
    private readonly HttpClient _httpClient;

    public CreateLeagueModel(HttpClient httpClient)
    {
        _httpClient = httpClient;
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
            var response = await _httpClient.PostAsync("https://api.fantasy-lcs.com/createleague", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Home");
            }
            else
            {
                // Read the response content as a string
                var errorMessage = await response.Content.ReadAsStringAsync();

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
            // Check if user has team
            if (!await UserHasTeam())
            {
                // Redirect to home page or a relevant page
                return RedirectToPage("/Home");
            }

            // Check if the user already has a league
            if (await UserHasLeague())
            {
                // Redirect to home page or a relevant page
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

    private async Task<bool> UserHasLeague()
    {
        var username = User.Identity.Name;

        // Make a request to check if the user has a team
        var response = await _httpClient.GetAsync($"https://api.fantasy-lcs.com/getleaguebyusername/{username}");
        return response.IsSuccessStatusCode;
    }

    private async Task<bool> UserHasTeam()
    {
        var username = User.Identity.Name;

        // Make a request to check if the user has a team
        var response = await _httpClient.GetAsync($"https://api.fantasy-lcs.com/getteambyusername/{username}");
        return response.IsSuccessStatusCode;
    }
}
