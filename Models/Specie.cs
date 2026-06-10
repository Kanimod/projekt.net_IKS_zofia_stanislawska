namespace Fischt.Models{
public class Specie
{
    public int Id { get; set; }

    public string ?Name { get; set; }
    
    public ICollection<Profile> Profiles { get; set; } = new List<Profile>();
}
}