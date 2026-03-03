namespace UcareBackApp.Cards.Entities;
public class Card
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Address { get; set; }   
    public string? Occupation { get; set; }
    public string? Description { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public required Guid UserId { get; set; } 
    public string? ImageUrl { get; set; }
}