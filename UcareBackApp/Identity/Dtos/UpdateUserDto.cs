using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UcareBackApp.Dtos
{
    public class UpdateUserDto
    {

        public required string CurrentEmail { get; set; }
        public required string NewName { get; set; }
        public required string NewEmail { get; set; }
    }
}
