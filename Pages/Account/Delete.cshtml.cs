using Fischt.Models;
using Fischt.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fischt.Pages.Account
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly IProfileRepository _profileRepo;
        private readonly IMatchRepository _matchRepo;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public DeleteModel(
            IProfileRepository profileRepo,
            IMatchRepository matchRepo,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _profileRepo = profileRepo;
            _matchRepo = matchRepo;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult OnGet() => Page();

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToPage("/Index");

            await _matchRepo.DeleteAllForUserAsync(user.Id!);

            await _profileRepo.DeleteAsync(user.Id!);

            await _signInManager.SignOutAsync();
            await _userManager.DeleteAsync(user);

            return RedirectToPage("/Index");
        }
    }
}