using Fischt.DTOs;
using Fischt.Models;
using Fischt.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fischt.Pages.Browse
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IProfileRepository _profileRepo;
        private readonly IMatchRepository _matchRepo;

        public IndexModel(UserManager<User> userManager, IProfileRepository profileRepo, IMatchRepository matchRepo)
        {
            _userManager = userManager;
            _profileRepo = profileRepo;
            _matchRepo = matchRepo;
        }

        public ProfileDto? CurrentProfile { get; set; }
        public bool NoMoreProfiles { get; set; } = false;

        public async Task OnGetAsync()
        {
            var me = await _userManager.GetUserAsync(User);
            if (me == null) return;

            var seen = await _matchRepo.GetSeenUserIdsAsync(me.Id!);
            var matched = (await _matchRepo.GetMatchesAsync(me.Id!))
                .Select(c => c.UserId == me.Id ? c.ContactId! : c.UserId!)
                .ToList();

            var skip = seen.Concat(matched).Distinct().ToList();
            skip.Add(me.Id!);

            var unseen = await _profileRepo.GetUnseen(me.Id!, skip);
            CurrentProfile = unseen.FirstOrDefault();
            NoMoreProfiles = CurrentProfile == null;
        }

        public async Task<IActionResult> OnPostLikeAsync(string targetUserId)
        {
            var me = await _userManager.GetUserAsync(User);
            if (me == null) return RedirectToPage();

            await _matchRepo.AddInviteAsync(me.Id!, targetUserId);

            if (await _matchRepo.HasReverseInviteAsync(me.Id!, targetUserId))
            {
                await _matchRepo.CreateMatchAsync(me.Id!, targetUserId);
                TempData["MatchMessage"] = "It is a match";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostPassAsync(string targetUserId)
        {
            var me = await _userManager.GetUserAsync(User);
            if (me == null) return RedirectToPage();

            await _matchRepo.AddInviteAsync(me.Id!, targetUserId);
            return RedirectToPage();
        }
    }
}