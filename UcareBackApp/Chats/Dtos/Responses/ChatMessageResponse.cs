using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UcareBackApp.Chats.Dtos.Responses
{
    public class ChatMessageResponse
    {
        public Guid ChatId { get; set; }
        public required string Message { get; set; }
    }
}