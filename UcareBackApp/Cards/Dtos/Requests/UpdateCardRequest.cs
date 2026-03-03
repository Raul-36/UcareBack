using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UcareBackApp.Cards.Dtos.Requests
{
    public class UpdateCardRequest
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Address { get; set; }
        public string? Occupation { get; set; }
        public string? Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}