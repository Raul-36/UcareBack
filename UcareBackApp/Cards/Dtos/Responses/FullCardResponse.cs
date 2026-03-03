using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UcareBackApp.Cards.Dtos.Responses
{
    public class FullCardResponse
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
}