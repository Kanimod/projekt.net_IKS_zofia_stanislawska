using Fischt.DTOs;

namespace Fischt.Repositories
{
    public interface IModerateProfileRepository
    {
        Task<List<ModerateProfileDto>> GetAllProfilesAsync();
        
        Task DeletePhotoAsync(string userId);
        
        Task DeleteProfileWithUserAsync(string userId);
    }
}