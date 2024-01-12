using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FantasyLCS.WebApp.Models
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginInputModel Input { get; set; }

        [BindProperty]
        public NewUserModel NewUser { get; set; }

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            // Any initialization before showing the form
        }

        public async Task<IActionResult> OnPostLoginAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // Return the same page if validation fails
            }

            // Implement your login logic here
            // For example: validate the Input.Username and Input.Password against your user store

            bool isUserValid = true;

            if (!isUserValid)
            {
                ErrorMessage = "Invalid username or password.";
                return Page();
            }

            // Handle authentication here
            // For example: creating authentication cookie, setting up session, etc.

            // Redirect to another page upon successful login
            return RedirectToPage("/Index"); // Change '/Index' to your successful login landing page
        }

        public async Task<IActionResult> OnPostSignupAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // Return the same page if validation fails
            }

            // Implement your login logic here
            // For example: validate the Input.Username and Input.Password against your user store

            bool isUserValid = true;

            if (!isUserValid)
            {
                ErrorMessage = "Invalid username or password.";
                return Page();
            }

            // Handle authentication here
            // For example: creating authentication cookie, setting up session, etc.

            // Redirect to another page upon successful login
            return RedirectToPage("/Index"); // Change '/Index' to your successful login landing page
        }

    }

    public class LoginInputModel
    {
        [Required(ErrorMessage = "Please enter your username.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter your password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class NewUserModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        // Add more properties as necessary
    }
}
