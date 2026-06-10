using Fischt.Models;

namespace Fischt.Repositories
{
    public interface IMatchRepository
    {
        Task AddInviteAsync(string senderId, string receiverId);
        Task<bool> HasReverseInviteAsync(string myUserId, string targetUserId);
        Task<List<string>> GetSeenUserIdsAsync(string myUserId);
        Task<Contact> CreateMatchAsync(string userId1, string userId2);
        Task<List<Contact>> GetMatchesAsync(string userId);
        Task DeleteAllForUserAsync(string userId);
    }
}