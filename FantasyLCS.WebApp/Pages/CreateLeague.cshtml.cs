using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace FantasyLCS.WebApp.Pages;

public class CreateLeagueModel : PageModel
{
    // Other properties and methods

    public async Task<IActionResult> OnPostCreate(string leagueName)
    {
        // Logic to create the league
        // Save the league name and other relevant information

        // After creating the league, redirect to the Home page
        return RedirectToPage("/Home");
    }
}
