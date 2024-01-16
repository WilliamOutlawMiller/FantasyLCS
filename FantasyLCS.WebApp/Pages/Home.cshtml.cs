using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using FantasyLCS.DataObjects;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace FantasyLCS.WebApp.Pages;

public class HomeModel : PageModel
{
    private readonly HttpClient _httpClient;

    public HomePage HomePage { get; set; }

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

            var response = await _httpClient.GetAsync($"https://api.fantasy-lcs.com/gethomepage/{username}");

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                HomePage = JsonSerializer.Deserialize<HomePage>(responseBody);
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
