using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using FantasyLCS.DataObjects;
using System.Text.Json;

namespace FantasyLCS.WebApp.Pages;

public class HomeModel : PageModel
{
    private readonly HttpClient _httpClient;

    // Add a property to store the team status
    public bool HasTeam { get; private set; }

    public Team UserTeam { get; private set; }

    public HomeModel(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task OnGet()
    {
        // Replace "username" with the actual username
        var username = User.Identity.Name;

        // Make a request to check if the user has a team
        var response = await _httpClient.GetAsync($"https://api.fantasy-lcs.com/getteambyusername/{username}");

        if (response.IsSuccessStatusCode)
        {
            // User has a team associated with their username
            HasTeam = true;

            var responseBody = await response.Content.ReadAsStringAsync();
            UserTeam = JsonSerializer.Deserialize<Team>(responseBody);
        }
        else
        {
            // User does not have a team
            HasTeam = false;
        }
    }
}
