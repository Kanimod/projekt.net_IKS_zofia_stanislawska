using Fischt.DTOs;
using Fischt.Models;
using Microsoft.EntityFrameworkCore;

namespace Fischt.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly FischtDbContext _context;

        public ProfileRepository(FischtDbContext context)
        {
            _context = context;
        }

        private static ProfileDto ToDto(Profile p) => new()
        {
            Id = p.Id,
            UserId = p.UserId,
            Name = p.Name,
            Age = p.Age,
            Bio = p.Bio,
            PhotoPath = p.PhotoPath,
            Gender = p.Gender,
            Sex = p.Sex,
            Pronouns = p.Pronouns,
            Preferences = p.Preferences,
            SpecieName = p.Specie?.Name,
            SpecieId = p.SpecieId,
            Length = p.Length
        };

        public async Task<List<ProfileDto>> GetAllAsync()
        {
            var profiles = await _context.Profiles
                .Include(p => p.Specie)
                .ToListAsync();
            return profiles.Select(ToDto).ToList();
        }

        public async Task<ProfileDto?> GetByUserIdAsync(string userId)
        {
            var profile = await _context.Profiles
                .Include(p => p.Specie)
                .FirstOrDefaultAsync(p => p.UserId == userId);
            return profile == null ? null : ToDto(profile);
        }

        public async Task<List<ProfileDto>> GetUnseen(string myUserId, List<string> seenUserIds)
        {
            var profiles = await _context.Profiles
                .Include(p => p.Specie)
                .Where(p => p.UserId != myUserId && !seenUserIds.Contains(p.UserId))
                .ToListAsync();
            return profiles.Select(ToDto).ToList();
        }

        public async Task CreateAsync(string userId, ProfileDto dto)
        {
            _context.Profiles.Add(new Profile
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                Name = dto.Name,
                Age = dto.Age,
                Bio = dto.Bio,
                Gender = dto.Gender,
                Sex = dto.Sex,
                Pronouns = dto.Pronouns,
                Preferences = dto.Preferences ?? "",
                SpecieId = dto.SpecieId,
                Length = dto.Length,
                PhotoPath = null
            });
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(string userId, ProfileDto dto)
        {
            var profile = await _context.Profiles
                .FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile == null) return;

            profile.Name = dto.Name;
            profile.Age = dto.Age;
            profile.Bio = dto.Bio;
            profile.PhotoPath = dto.PhotoPath;
            profile.Gender = dto.Gender;
            profile.Sex = dto.Sex;
            profile.Pronouns = dto.Pronouns;
            profile.Preferences = dto.Preferences;
            profile.SpecieId = dto.SpecieId;
            profile.Length = dto.Length;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string userId)
        {
            var profile = await _context.Profiles
                .FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile == null) return;
            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();
        }
    }
}