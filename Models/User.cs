using Microsoft.AspNetCore.Identity;
namespace Fischt.Models{
public class User : IdentityUser 
{
    public bool? Premium { get; set; }


    public Profile? Profile { get; set; }
    public ICollection<Invite> SentInvites { get; set; } = new List<Invite>();
    public ICollection<Invite> ReceivedInvites { get; set; } = new List<Invite>();
    public ICollection<Contact> UserContacts { get; set; } = new List<Contact>();
    public ICollection<Contact> ContactOfUsers { get; set; } = new List<Contact>();
    public ICollection<Message> Messages { get; set; } = new List<Message>();
    public ICollection<UserInterest> UserInterests { get; set; }= new List<UserInterest>();
}
}