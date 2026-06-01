namespace Fischt.Models{
public class Specie
{
    public int Id { get; set; }
    public string? WaterFlavour { get; set; }
    public string? Depth { get; set; }
    public string? BreedingSeason { get; set; }

    
    public ICollection<Profile> Profiles { get; set; } = new List<Profile>();
}
}