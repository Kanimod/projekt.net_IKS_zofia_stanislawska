using Fischt.DTOs;
using Fischt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Fischt.Repositories
{
    public class ModerateProfileRepository : IModerateProfileRepository
    {
        private readonly FischtDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _environment;
        private readonly IMatchRepository _matchRepo;
        private readonly IProfileRepository _profileRepo;

        public ModerateProfileRepository(
            FischtDbContext context,
            UserManager<User> userManager,
            IWebHostEnvironment environment,
            IMatchRepository matchRepo,
            IProfileRepository profileRepo)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
            _matchRepo = matchRepo;
            _profileRepo = profileRepo;
        }

        public async Task<List<ModerateProfileDto>> GetAllProfilesAsync()
        {
            var profiles = await _context.Profiles
                .Include(p => p.User)
                .Include(p => p.Specie)
                .ToListAsync();

            return profiles.Select(p => new ModerateProfileDto
            {
                UserId = p.UserId,
                Email = p.User?.Email,
                Name = p.Name,
                Age = p.Age,
                PhotoPath = p.PhotoPath,
                Gender = p.Gender,
                Sex = p.Sex,
                SpecieName = p.Specie?.Name,
                Bio = p.Bio
            }).ToList();
        }

        public async Task DeletePhotoAsync(string userId)
        {
            var profile = await _context.Profiles
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (profile == null || string.IsNullOrEmpty(profile.PhotoPath))
                return;

            // Usuń fizyczny plik z dysku
            var filePath = Path.Combine(
                _environment.WebRootPath,
                profile.PhotoPath.TrimStart('/'));

            if (File.Exists(filePath))
                File.Delete(filePath);

            // Wyczyść ścieżkę w bazie
            profile.PhotoPath = null;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProfileWithUserAsync(string userId)
        {
            await DeletePhotoAsync(userId);

            await _matchRepo.DeleteAllForUserAsync(userId);

            await _profileRepo.DeleteAsync(userId);

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
                await _userManager.DeleteAsync(user);
        }
    }
}