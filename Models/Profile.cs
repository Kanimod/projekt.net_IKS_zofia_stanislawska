namespace Fischt.Models{
public class Profile
    {
    public string? Id { get; set; }
    public string? UserId { get; set; } 
    public string? Gender { get; set; }
    public string? Sex { get; set; }
    public string? Preferences { get; set; }
    public string? Pronouns { get; set; }
    public int? SpecieId { get; set; } 
    public float? Length { get; set; }


    public User? User { get; set; }
    public Specie? Specie { get; set; }
}
}