using Fischt.Models;
using Microsoft.EntityFrameworkCore;

namespace Fischt.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly FischtDbContext _context;

        public MessageRepository(FischtDbContext context)
        {
            _context = context;
        }

        public async Task<List<Message>> GetByConversationAsync(string conversationId)
        {
            return await _context.Messages
                .Include(m => m.Sender)
                .Where(m => m.ConversationId == conversationId)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }

        public async Task SendAsync(string conversationId, string senderId, string text)
        {
            _context.Messages.Add(new Message
            {
                Id = Guid.NewGuid().ToString(),
                ConversationId = conversationId,
                SenderId = senderId,
                Text = text,
                Timestamp = DateTime.UtcNow.ToString("o"),
                State = "sent"
            });
            await _context.SaveChangesAsync();
        }
    }
}