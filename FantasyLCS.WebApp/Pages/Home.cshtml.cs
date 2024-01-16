using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using FantasyLCS.DataObjects;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace FantasyLCS.WebApp.Pages;

public class HomeModel : PageModel
{
    private readonly HttpClient _httpClient;

    public Team UserTeam { get; private set; }

    public League UserLeague { get; private set; }

    public List<Team> LeagueTeams { get; private set; } = new List<Team>();

    public HomeModel(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IActionResult> OnGet()
    {
        // todo: we can update the API with an endpoint called LoadHomePage that returns all the info we need in one request.

        if (User.Identity.IsAuthenticated)
        {
            var username = User.Identity.Name;

            var teamResponse = await _httpClient.GetAsync($"https://api.fantasy-lcs.com/getteambyusername/{username}");

            if (teamResponse.IsSuccessStatusCode)
            {
                var teamResponseBody = await teamResponse.Content.ReadAsStringAsync();
                UserTeam = JsonSerializer.Deserialize<Team>(teamResponseBody);
            }

            var leagueResponse = await _httpClient.GetAsync($"https://api.fantasy-lcs.com/getleaguebyusername/{username}");

            if (leagueResponse.IsSuccessStatusCode)
            {
                var leagueResponseBody = await leagueResponse.Content.ReadAsStringAsync();
                UserLeague = JsonSerializer.Deserialize<League>(leagueResponseBody);
            }

            if (UserLeague != null)
            {
                var teamsResponse = await _httpClient.GetAsync($"https://api.fantasy-lcs.com/getteamsbyleagueid/{UserLeague.ID}");
                if (teamsResponse.IsSuccessStatusCode)
                {
                    var teamsResponseBody = await teamsResponse.Content.ReadAsStringAsync();
                    LeagueTeams = JsonSerializer.Deserialize<List<Team>>(teamsResponseBody);
                }
            }

            return Page();
        }
        else
        {
            // If user is not authenticated, redirect to login page
            return RedirectToPage("/Login");
        }
    }
}
