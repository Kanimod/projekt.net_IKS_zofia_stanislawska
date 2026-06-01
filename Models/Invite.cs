namespace Fischt.Models{
public class Invite
{
        public string? SenderId { get; set; } 
        public string? ReceiverId { get; set; } 


        public User? Sender { get; set; }
        public User? Receiver { get; set; }
}
}
