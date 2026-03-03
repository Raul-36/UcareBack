using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace UcareBackApp.Cards.Dtos.Requests
{
    public class CreateCardRequest
    {
        public required string Name { get; set; }
        public string? Address { get; set; }
        public string? Occupation { get; set; }
        public string? Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public IFormFile? Image { get; set; }
    }
}
