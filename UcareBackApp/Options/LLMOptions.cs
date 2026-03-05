using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UcareBackApp.Options
{
    public class LLMOptions
    {
        public required string ApiKey { get; set; } 
        public required string ModelId { get; set; } 
    }
}