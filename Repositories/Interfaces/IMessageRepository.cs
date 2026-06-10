using Fischt.Models;

namespace Fischt.Repositories
{
    public interface IMessageRepository
    {
        Task<List<Message>> GetByConversationAsync(string conversationId);
        Task SendAsync(string conversationId, string senderId, string text);
    }
}