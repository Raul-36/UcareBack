using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UcareBackApp.Chats.Dtos.Requests
{
    public class SendMessageRequest
    {
        public required string Message { get; set; }
    }
}