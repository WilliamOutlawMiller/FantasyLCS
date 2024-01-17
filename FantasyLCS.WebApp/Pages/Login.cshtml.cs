using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FantasyLCS.WebApp.Pages
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public LoginModel(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiUrl = configuration["ApiSettings:BaseUrl"];
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
                LoginRequest loginData = new LoginRequest
                {
                    Username = Input.Username,
                    Password = Input.Password
                };

                // Serialize the data to JSON
                var jsonRequest = JsonSerializer.Serialize(loginData);

                // Create a request content with JSON data
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                // Send a POST request to the login API endpoint
                var response = await _httpClient.PostAsync(_apiUrl + "/login", content);

                // Check if the request was successful (you can customize this based on your API response format)
                if (response.IsSuccessStatusCode)
                {
                    // Create claims for the authenticated user
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, Input.Username),
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));

                    return RedirectToPage("/Home"); // Replace with your success page
                }
                else
                {
                    // Handle the login failure
                    var errorMessage = await response.Content.ReadAsStringAsync();

                    // Check if the response contains an error message
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        ModelState.AddModelError("Error: ", errorMessage);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Login failed. Please try again.");
                    }

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
                SignupRequest signupData = new SignupRequest
                {
                    Username = NewUser.Username,
                    Password = NewUser.Password
                };

                // Serialize the data to JSON
                var jsonRequest = JsonSerializer.Serialize(signupData);

                // Create a request content with JSON data
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                // Send a POST request to the signup API endpoint
                var response = await _httpClient.PostAsync(_apiUrl + "/signup", content);

                if (response.IsSuccessStatusCode)
                {
                    // Create claims for the authenticated user
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, NewUser.Username),
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));

                    // Redirect to a success page or perform other actions as needed
                    return RedirectToPage("/Home"); // Replace with your success page
                }
                else
                {
                    // Handle the login failure
                    var errorMessage = await response.Content.ReadAsStringAsync();

                    // Check if the response contains an error message
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        ModelState.AddModelError("Error: ", errorMessage);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Signup failed. Please try again.");
                    }

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
