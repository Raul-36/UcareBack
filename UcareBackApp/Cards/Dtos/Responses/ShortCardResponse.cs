using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UcareBackApp.Cards.Dtos.Responses
{
    public class ShortCardResponse
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Address { get; set; }   
        public string? Occupation { get; set; } 
        public string? ImageUrl { get; set; }
    }
}