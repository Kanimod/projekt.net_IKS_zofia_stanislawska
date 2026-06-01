namespace Fischt.Models
{
public class Contact
{
    public string? Id { get; set; }
    public string? UserId { get; set; } 
    public string? ContactId { get; set; } 


    public User? User { get; set; }
    public User? ContactUser { get; set; }
    public ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();
}
}