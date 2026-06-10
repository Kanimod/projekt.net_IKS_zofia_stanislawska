using Fischt.DTOs;
using Fischt.Repositories;
using Fischt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Fischt.Pages.Profiles
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IProfileRepository _profileRepo;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _environment;
        private readonly FischtDbContext _context;

        public EditModel(IProfileRepository profileRepo, UserManager<User> userManager,
            IWebHostEnvironment environment, FischtDbContext context)
        {
            _profileRepo = profileRepo;
            _userManager = userManager;
            _environment = environment;
            _context = context;
        }

        [BindProperty]
        public ProfileDto Profile { get; set; } = new();

        [BindProperty]
        public IFormFile? PhotoUpload { get; set; }

        public SelectList? SpeciesList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var me = await _userManager.GetUserAsync(User);
            if (me == null) return RedirectToPage("/Index");

            var profile = await _profileRepo.GetByUserIdAsync(me.Id!);
            if (profile == null) return RedirectToPage("/Index");

            Profile = profile;
            SpeciesList = new SelectList(_context.Species.ToList(), "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var me = await _userManager.GetUserAsync(User);
            if (me == null) return RedirectToPage("/Index");

            // Obsługa zdjęcia
            if (PhotoUpload != null && PhotoUpload.Length > 0)
            {
                var allowed = new[] { "image/jpeg", "image/png", "image/webp" };
                if (!allowed.Contains(PhotoUpload.ContentType))
                {
                    ModelState.AddModelError("PhotoUpload", "Allowed: jpg, png, webp");
                    SpeciesList = new SelectList(_context.Species.ToList(), "Id", "Name");
                    return Page();
                }

                var ext = Path.GetExtension(PhotoUpload.FileName);
                var fileName = $"{me.Id}{ext}";
                var path = Path.Combine(_environment.WebRootPath, "uploads", fileName);

                using var stream = new FileStream(path, FileMode.Create);
                await PhotoUpload.CopyToAsync(stream);

                Profile.PhotoPath = $"/uploads/{fileName}";
            }

            await _profileRepo.UpdateAsync(me.Id!, Profile);

            TempData["Success"] = "Profile updated!";
            return RedirectToPage();
        }
    }
}