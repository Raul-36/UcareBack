using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using UcareBackApp.Chats.Entities;
using UcareBackApp.Chats.Repositories.Base;
using UcareBackApp.Data;

namespace UcareBackApp.Chats.Repositories
{
    public class ChatEFRepository : IChatRepository
    {
        private readonly UcareDbContext context;

        public ChatEFRepository(UcareDbContext context)
        {
            this.context = context;
        }

        public async Task AddMessageToChatAsync(Guid chatId, Message message)
        {
            var chat = context.Chats.FirstOrDefault(c => c.Id == chatId);
            if (chat is null)
                throw new KeyNotFoundException("Chat not found for the given chat ID.");
            
            chat.Messages =chat.Messages.Append(message).ToList();
            await context.SaveChangesAsync();
        }

        public async Task<Chat> CreateChatAsync(Chat chat)
        {
            await context.Chats.AddAsync(chat);
            await context.SaveChangesAsync();
            return chat;
        }

        public async Task DeleteChatAsync(Guid chatId)
        {
            var chat = context.Chats.FirstOrDefault(c => c.Id == chatId);
            if ((chat is null) == false)
            {
                context.Chats.Remove(chat);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Chat?> GetChatByIdAsync(Guid chatId)
        {
            var chat = await context.Chats.AsNoTracking().FirstOrDefaultAsync(c => c.Id == chatId);
            return chat;
        }

        public async Task<IEnumerable<Chat>> GetChatsByCardIdAsync()
        {
            return await context.Chats.AsNoTracking().Select(c => new Chat
            {
                Id = c.Id,
                CardId = c.CardId,
                Messages = c.Messages.Skip(2).Take(1).ToList()
            }
            ).ToListAsync();
        }
    }
}
