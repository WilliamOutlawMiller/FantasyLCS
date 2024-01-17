using FantasyLCS.DataObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _cache;
        private readonly string _apiUrl;

        public CreateTeamModel(HttpClient httpClient, IMemoryCache memoryCache, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _cache = memoryCache;
            _apiUrl = configuration["ApiSettings:BaseUrl"];
        }

        [BindProperty]
        public string TeamName { get; set; }

        [BindProperty]
        public string TeamLogoURL { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                string username = User.Identity.Name;
                // Prepare the data for creating a team
                CreateTeamRequest teamData = new CreateTeamRequest
                {
                    Name = TeamName,
                    Username = username,
                    LogoUrl = TeamLogoURL
                };

                // Serialize the data to JSON
                var jsonRequest = JsonSerializer.Serialize(teamData);

                // Create a request content with JSON data
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                // Send a POST request to the create team API endpoint
                var response = await _httpClient.PostAsync(_apiUrl + "/createteam", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Team userTeam = JsonSerializer.Deserialize<Team>(responseBody);

                    string cacheKey = $"HomePageData-{username}";
                    HomePage cachedHomePage;

                    if (_cache.TryGetValue(cacheKey, out cachedHomePage))
                    {
                        // Update the UserTeam property with the new team details.
                        cachedHomePage.UserTeam = userTeam;

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
                string cacheKey = $"HomePageData-{User.Identity.Name}";
                HomePage cachedHomePage;

                if (_cache.TryGetValue(cacheKey, out cachedHomePage))
                {
                    if (cachedHomePage.UserTeam != null)
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
}
