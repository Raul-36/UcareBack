using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.AI;
using UcareBackApp.Cards.Entities;

namespace UcareBackApp.Chats.Entities
{
    public class Chat
    {
        public Guid Id { get; set; }
        public Guid? CardId { get; set; }
        public required List<Message> Messages { get; set; }
    }
}
