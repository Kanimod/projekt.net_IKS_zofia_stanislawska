using Fischt.Models;
using Fischt.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fischt.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ManageSpeciesModel : PageModel
    {
        private readonly ISpecieRepository _specieRepo;

        public ManageSpeciesModel(ISpecieRepository specieRepo)
        {
            _specieRepo = specieRepo;
        }

        public List<Specie> Species { get; set; } = new();
        
        [BindProperty(SupportsGet = true)]
        public int? EditId { get; set; }

        public async Task OnGetAsync()
        {
            Species = await _specieRepo.GetAllAsync();
        }

        public async Task<IActionResult> OnPostAddAsync(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                await _specieRepo.CreateAsync(name);
                TempData["Success"] = $"Species '{name}' added";
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateAsync(int specieId, string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                await _specieRepo.UpdateAsync(specieId, name);
                TempData["Success"] = $"Species updated";
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int specieId)
        {
            await _specieRepo.DeleteAsync(specieId);
            TempData["Success"] = "Species deleted";
            return RedirectToPage();
        }
    }
}