using Fischt.DTOs;
using Fischt.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fischt.Pages.Moderate
{
    [Authorize(Roles = "Admin")]
    public class ManageProfilesModel : PageModel
    {
        private readonly IModerateProfileRepository _adminRepo;

        public ManageProfilesModel(IModerateProfileRepository adminRepo)
        {
            _adminRepo = adminRepo;
        }

        public List<ModerateProfileDto> Profiles { get; set; } = new();

        public async Task OnGetAsync()
        {
            Profiles = await _adminRepo.GetAllProfilesAsync();
        }

        // Usuń tylko zdjęcie – profil i konto zostają
        public async Task<IActionResult> OnPostDeletePhotoAsync(string userId)
        {
            await _adminRepo.DeletePhotoAsync(userId);
            TempData["Success"] = "Photo deleted";
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostDeleteProfileAsync(string userId)
        {
            await _adminRepo.DeleteProfileWithUserAsync(userId);
            TempData["Success"] = "Profile and account deleted";
            return RedirectToPage();
        }
    }
}