using Fischt.Models;
using Fischt.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fischt.Pages.Chat
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IMatchRepository _matchRepo;
        private readonly IMessageRepository _messageRepo;

        public IndexModel(UserManager<User> userManager, IMatchRepository matchRepo, IMessageRepository messageRepo)
        {
            _userManager = userManager;
            _matchRepo = matchRepo;
            _messageRepo = messageRepo;
        }

        public List<ContactViewModel> Matches { get; set; } = new();
        public List<Message> Messages { get; set; } = new();
        public string? OpenConversationId { get; set; }
        public string? OpenWithUserName { get; set; }
        public string? CurrentUserId { get; set; }

        [BindProperty]
        public string? NewMessageText { get; set; }

        public async Task OnGetAsync(string? conversationId)
        {
            var me = await _userManager.GetUserAsync(User);
            if (me == null) return;

            CurrentUserId = me.Id;

            var contacts = await _matchRepo.GetMatchesAsync(me.Id!);
            foreach (var contact in contacts)
            {
                var other = contact.UserId == me.Id ? contact.ContactUser : contact.User;
                var conv = contact.Conversations?.FirstOrDefault();
                Matches.Add(new ContactViewModel
                {
                    ContactId = contact.Id,
                    ConversationId = conv?.Id,
                    OtherUserName = other?.UserName ?? "Unknown",
                    OtherUserId = other?.Id
                });
            }

            if (conversationId != null)
            {
                OpenConversationId = conversationId;
                Messages = await _messageRepo.GetByConversationAsync(conversationId);
                OpenWithUserName = Matches.FirstOrDefault(m => m.ConversationId == conversationId)?.OtherUserName;
            }
        }

        public async Task<IActionResult> OnPostSendAsync(string conversationId)
        {
            var me = await _userManager.GetUserAsync(User);
            if (me == null || string.IsNullOrWhiteSpace(NewMessageText))
                return RedirectToPage(new { conversationId });

            await _messageRepo.SendAsync(conversationId, me.Id!, NewMessageText.Trim());
            return RedirectToPage(new { conversationId });
        }
    }

    public class ContactViewModel
    {
        public string? ContactId { get; set; }
        public string? ConversationId { get; set; }
        public string? OtherUserName { get; set; }
        public string? OtherUserId { get; set; }
    }
}