using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UcareBackApp.Chats.Entities
{
    public class Message
    {
        public required string Role { get; set; }
        public required string Content { get; set; }
    }
}