using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UcareBackApp.Dtos
{
    public class RegisterDto
    {
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
