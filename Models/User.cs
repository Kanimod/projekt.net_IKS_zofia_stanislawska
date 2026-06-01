namespace Fischt.Models{
public class User
{
    public string? Id { get; set; } 
    public string? Username { get; set; }
    public bool Admin { get; set; }
    public string? Mail { get; set; }
    public string? PasswordHash { get; set; }
    public bool Premium { get; set; }


    public Profile? Profile { get; set; }
    public ICollection<Invite> SentInvites { get; set; } = new List<Invite>();
    public ICollection<Invite> ReceivedInvites { get; set; } = new List<Invite>();
    public ICollection<Contact> UserContacts { get; set; } = new List<Contact>();
    public ICollection<Contact> ContactOfUsers { get; set; } = new List<Contact>();
    public ICollection<Message> Messages { get; set; } = new List<Message>();
}
}