using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UcareBackApp.Chats.Dtos.Responses
{
    public class ChatResponse
    {
        public Guid Id { get; set; }
        public Guid? CardId { get; set; }
        public required IEnumerable<ChatMessagesItem> Messages { get; set; }
    }
}