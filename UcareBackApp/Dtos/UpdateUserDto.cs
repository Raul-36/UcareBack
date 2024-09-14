using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UcareBackApp.Dtos
{
    public class UpdateUserDto
    {

        public string CurrentEmail { get; set; }
        public string NewName { get; set; }
        public string NewEmail { get; set; }
    }
}
