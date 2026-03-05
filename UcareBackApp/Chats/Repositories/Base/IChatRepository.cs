using Microsoft.Extensions.AI;
using UcareBackApp.Chats.Entities;

namespace UcareBackApp.Chats.Repositories.Base
{
    public interface IChatRepository
    {
        Task<IEnumerable<Chat>> GetChatsByCardIdAsync();
        Task<Chat?> GetChatByIdAsync(Guid chatId);
        Task<Chat> CreateChatAsync(Chat chat);
        Task AddMessageToChatAsync(Guid cardId, Message message);
        Task DeleteChatAsync(Guid chatId);
    }
}
