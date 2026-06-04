namespace Fischt.Models
{
    public class UserInterest
    {
        public string? UserId { get; set; }
        public User? User { get; set; }
        public string? InterestId { get; set; }
        public Interest? Interest { get; set; }


    }
}