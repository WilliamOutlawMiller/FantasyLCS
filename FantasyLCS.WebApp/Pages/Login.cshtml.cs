using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FantasyLCS.WebApp.Pages
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public LoginModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [BindProperty]
        public LoginInputModel Input { get; set; }

        [BindProperty]
        public SignupInputModel NewUser { get; set; }

        // Handle the login button click
        public async Task<IActionResult> OnPostLogin()
        {
            try
            {
                // Prepare the login request data
                var loginData = new
                {
                    Username = Input.Username,
                    Password = Input.Password
                };

                // Serialize the data to JSON
                var jsonRequest = JsonSerializer.Serialize(loginData);

                // Create a request content with JSON data
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                // Send a POST request to the login API endpoint
                var response = await _httpClient.PostAsync("https://api.fantasy-lcs.com/login", content);

                // Check if the request was successful (you can customize this based on your API response format)
                if (response.IsSuccessStatusCode)
                {
                    // Redirect to a success page or perform other actions as needed
                    return RedirectToPage("/SuccessPage"); // Replace with your success page
                }
                else
                {
                    // Handle the login failure, display an error message, or perform other actions
                    ModelState.AddModelError(string.Empty, "Login failed. Please try again.");
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

        // Handle the signup button click
        public async Task<IActionResult> OnPostSignup()
        {
            try
            {
                // Prepare the signup request data
                var signupData = new
                {
                    Username = NewUser.Username,
                    Password = NewUser.Password
                };

                // Serialize the data to JSON
                var jsonRequest = JsonSerializer.Serialize(signupData);

                // Create a request content with JSON data
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                // Send a POST request to the signup API endpoint
                var response = await _httpClient.PostAsync("https://api.fantasy-lcs.com/signup", content);

                // Check if the request was successful (you can customize this based on your API response format)
                if (response.IsSuccessStatusCode)
                {
                    // Redirect to a success page or perform other actions as needed
                    return RedirectToPage("/SuccessPage"); // Replace with your success page
                }
                else
                {
                    // Handle the signup failure, display an error message, or perform other actions
                    ModelState.AddModelError(string.Empty, "Signup failed. Please try again.");
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

    public class LoginInputModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class SignupInputModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
