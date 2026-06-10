using Fischt.Models;
using Microsoft.EntityFrameworkCore;

namespace Fischt.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly FischtDbContext _context;

        public MatchRepository(FischtDbContext context)
        {
            _context = context;
        }

        public async Task AddInviteAsync(string senderId, string receiverId)
        {
            _context.Invites.Add(new Invite { SenderId = senderId, ReceiverId = receiverId });
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasReverseInviteAsync(string myUserId, string targetUserId)
        {
            return await _context.Invites
                .AnyAsync(i => i.SenderId == targetUserId && i.ReceiverId == myUserId);
        }

        public async Task<List<string>> GetSeenUserIdsAsync(string myUserId)
        {
            return await _context.Invites
                .Where(i => i.SenderId == myUserId)
                .Select(i => i.ReceiverId!)
                .ToListAsync();
        }

        public async Task<Contact> CreateMatchAsync(string userId1, string userId2)
        {
            var contact = new Contact
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId1,
                ContactId = userId2
            };
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            _context.Conversations.Add(new Conversation
            {
                Id = Guid.NewGuid().ToString(),
                ContactId = contact.Id
            });
            await _context.SaveChangesAsync();
            return contact;
        }

        public async Task<List<Contact>> GetMatchesAsync(string userId)
        {
            return await _context.Contacts
                .Include(c => c.User)
                .Include(c => c.ContactUser)
                .Include(c => c.Conversations)
                .Where(c => c.UserId == userId || c.ContactId == userId)
                .ToListAsync();
        }

        public async Task DeleteAllForUserAsync(string userId)
        {
            var contactIds = await _context.Contacts
                .Where(c => c.UserId == userId || c.ContactId == userId)
                .Select(c => c.Id).ToListAsync();

            var convIds = await _context.Conversations
                .Where(c => contactIds.Contains(c.ContactId))
                .Select(c => c.Id).ToListAsync();

            _context.Messages.RemoveRange(
                _context.Messages.Where(m => convIds.Contains(m.ConversationId)));
            _context.Conversations.RemoveRange(
                _context.Conversations.Where(c => contactIds.Contains(c.ContactId)));
            _context.Contacts.RemoveRange(
                _context.Contacts.Where(c => c.UserId == userId || c.ContactId == userId));
            _context.Invites.RemoveRange(
                _context.Invites.Where(i => i.SenderId == userId || i.ReceiverId == userId));

            await _context.SaveChangesAsync();
        }
    }
}