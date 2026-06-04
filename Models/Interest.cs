namespace Fischt.Models
{
    public class Interest
    {
        public string? Id { get; set; }
        public string? Name { get; set; }

        public ICollection<UserInterest> UserInterests { get; set; } = new List<UserInterest>();
    }
}