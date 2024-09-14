namespace UcareBackApp.Models;
public class Card
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Occupation { get; set; }
    public string Description { get; set; }
    public string ImgPath { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string UserEmail { get; set; } 
}