using Fischt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace projekt.net_IKS_zofia_stanislawska.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(SignInManager<User> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            
            // Ustaw komunikat sukcesu w TempData
            TempData["SuccessMessage"] = "Zostałeś pomyślnie wylogowany.";
            
            // Przekieruj na stronę główną (lub na returnUrl, jeśli podany)
            if (!string.IsNullOrEmpty(returnUrl))
                return LocalRedirect(returnUrl);
            else
                return RedirectToPage("/Index");
        }
    }
}