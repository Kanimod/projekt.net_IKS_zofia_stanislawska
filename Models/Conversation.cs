namespace Fischt.Models
{
public class Conversation
{
    public string? Id { get; set; }
    public string? ContactId { get; set; }


    public Contact? Contact { get; set; }
    public ICollection<Message> Messages { get; set; } = new List<Message>();
}
}