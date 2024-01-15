using FantasyLCS.DataObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FantasyLCS.WebApp.Pages
{
    public class CreateTeamModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public CreateTeamModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public string TeamName { get; set; }

        [BindProperty]
        public string TeamLogoURL { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                // Prepare the data for creating a team
                CreateTeamRequest teamData = new CreateTeamRequest
                {
                    Name = TeamName,
                    Username = User.Identity.Name,
                    LogoUrl = TeamLogoURL
                };

                // Serialize the data to JSON
                var jsonRequest = JsonSerializer.Serialize(teamData);

                // Create a request content with JSON data
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                // Send a POST request to the create team API endpoint
                var response = await _httpClient.PostAsync("https://api.fantasy-lcs.com/createteam", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("/Home");
                }
                else
                {
                    // Read the response content as a string
                    var errorMessage = await response.Content.ReadAsStringAsync();

                    // Add the error message to the ModelState
                    ModelState.AddModelError(string.Empty, $"Create team failed: {errorMessage}");
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
                // Check if the user already has a team
                if (await UserAlreadyHasTeam())
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

        private async Task<bool> UserAlreadyHasTeam()
        {
            var username = User.Identity.Name;

            // Make a request to check if the user has a team
            var response = await _httpClient.GetAsync($"https://api.fantasy-lcs.com/getteambyusername/{username}");
            return response.IsSuccessStatusCode;
        }

    }
}
