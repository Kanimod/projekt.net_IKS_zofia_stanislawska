namespace Fischt.DTOs
{
    public class ProfileDto
    {
        public string? Id { get; set; } 
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public int? Age { get; set; }
        public string? Bio { get; set; }
        public string? PhotoPath { get; set; }
        public string? Gender { get; set; }
        public string? Sex { get; set; }
        public string? Pronouns { get; set; }
        public string? Preferences { get; set; }
        public string? SpecieName { get; set; }
        public int? SpecieId { get; set; }
        public float? Length { get; set; }
    }
}