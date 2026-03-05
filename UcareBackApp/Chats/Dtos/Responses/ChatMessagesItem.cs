using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UcareBackApp.Chats.Dtos.Responses
{
    public class ChatMessagesItem
    {
        public required string Role { get; set; }
        public required string Content { get; set; }
    }
}