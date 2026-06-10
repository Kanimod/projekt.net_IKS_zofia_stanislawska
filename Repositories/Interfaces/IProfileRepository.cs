using Fischt.DTOs;

namespace Fischt.Repositories
{
    public interface IProfileRepository
    {
        Task<List<ProfileDto>> GetAllAsync();
        Task<ProfileDto?> GetByUserIdAsync(string userId);
        Task<List<ProfileDto>> GetUnseen(string myUserId, List<string> seenUserIds);
        Task CreateAsync(string userId, ProfileDto dto);  
        Task UpdateAsync(string userId, ProfileDto dto);
        Task DeleteAsync(string userId);
    }
}