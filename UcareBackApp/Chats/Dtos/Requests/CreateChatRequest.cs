using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UcareBackApp.Chats.Dtos.Requests
{
    public class CreateChatRequest
    {
        public Guid CardId { get; set; }
        public required string Message { get; set; }
    }
}