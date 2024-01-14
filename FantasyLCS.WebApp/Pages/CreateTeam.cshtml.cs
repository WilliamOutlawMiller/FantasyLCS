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
                var response = await _httpClient.PostAsync("https://localhost:7273/createteam", content);

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
    }
}
