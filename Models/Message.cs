namespace Fischt.Models{
public class Message
{
    public string? Id { get; set; }
    public string? ConversationId { get; set; } 
    public string? SenderId { get; set; } 
    public string? Text { get; set; }
    public string? State { get; set; }
    public string? Timestamp { get; set; }


    public Conversation? Conversation { get; set; }
    public User? Sender { get; set; }
}
}