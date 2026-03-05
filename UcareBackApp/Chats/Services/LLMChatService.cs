using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;
using UcareBackApp.Cards.Services.Base;
using UcareBackApp.Chats.Dtos.Requests;
using UcareBackApp.Chats.Dtos.Responses;
using UcareBackApp.Chats.Repositories.Base;
using UcareBackApp.Chats.Services.Base;


namespace UcareBackApp.Chats.Services
{
    public class LLMChatService : IChatService
    {
        private readonly ICardService cardService;
        private readonly IChatRepository chatRepository;
        private readonly IChatClient chatClient;
        private readonly string systemPrompt;
        public LLMChatService(ICardService cardService, IChatRepository chatRepository, IChatClient chatClient)
        {
            this.cardService = cardService;
            this.chatRepository = chatRepository;
            this.chatClient = chatClient;
            string path = Path.Combine(AppContext.BaseDirectory, "Resources", "systemPrompt.txt");
            if (!File.Exists(path))
                throw new FileNotFoundException($"File not found: {path}");
            this.systemPrompt = File.ReadAllText(path);
        }
        public async Task<ChatMessageResponse> SendMessageAsync(Guid chatId, SendMessageRequest request)
        {
            var chat = await chatRepository.GetChatByIdAsync(chatId);
            if (chat is null)
                throw new KeyNotFoundException("Chat not found");
            
            var messages = chat.Messages.Select(m => new ChatMessage(new ChatRole(m.Role), m.Content)).ToList();
            var userMessage = new ChatMessage(ChatRole.User, request.Message);
            messages.Add(userMessage);
            
            var response = await chatClient.GetResponseAsync(messages);

            var responseRole = response.Messages.First().Role.ToString().ToLower();

            var userMessageAsEntity = new Entities.Message { Role = userMessage.Role.ToString().ToLower(), Content = userMessage.Text };
            var responseAsEntity = new Entities.Message { Role = responseRole, Content = response.Text };
            
            await chatRepository.AddMessageToChatAsync(chat.Id, userMessageAsEntity);
            await chatRepository.AddMessageToChatAsync(chat.Id, responseAsEntity);

            return new ChatMessageResponse { Message = response.Text, ChatId = chat.Id };
        }

        public async Task DeleteChatAsync(Guid chatId)
        {
            await chatRepository.DeleteChatAsync(chatId);
        }

        public async Task<Dtos.Responses.ChatResponse> GetChatByIdAsync(Guid chatId)
        {
            var chat = await chatRepository.GetChatByIdAsync(chatId);
            if (chat is null)                
                throw new KeyNotFoundException("Chat not found");

            var response = new Dtos.Responses.ChatResponse
            {
                Id = chat.Id,
                CardId = chat.CardId,
                Messages = chat.Messages.Select(m => new ChatMessagesItem { Role = m.Role, Content = m.Content }).ToList()
            };
            return response;
        }
        public async Task<IEnumerable<Dtos.Responses.ChatResponse>> GetChatsAsync()
        {
            var chats = await chatRepository.GetChatsByCardIdAsync();
            var response = chats.Select(c => new Dtos.Responses.ChatResponse
            {
                Id = c.Id,
                CardId = c.CardId,
                Messages = c.Messages.Where(m => m.Role.ToLower() != "system")
                    .Select(m => new ChatMessagesItem { Role = m.Role, Content = m.Content }).ToList()
            });
            return response;
        }

        public async Task<ChatMessageResponse> CreateChatAsync(CreateChatRequest request)
        {
            var card = await this.cardService.GetCardAsync(request.CardId);
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            var cardJson = JsonSerializer.Serialize(card, options);
            var messages = new List<ChatMessage>
            {
                new ChatMessage (ChatRole.System, systemPrompt),
                new ChatMessage (ChatRole.System, "Card info :" + cardJson),
                new ChatMessage (ChatRole.User,request.Message)
            };

            var response = await chatClient.GetResponseAsync(messages);

            var chat = new Entities.Chat
            {
                CardId = request.CardId,
                Messages = messages.Select(m => new Entities.Message { Role = m.Role.ToString().ToLower(), Content = m.Text }).ToList()
            };
            var mesRole = response.Messages.First().Role.ToString().ToLower();
            chat.Messages.Add(new Entities.Message { Role = mesRole, Content = response.Text });
            await chatRepository.CreateChatAsync(chat);
            return new ChatMessageResponse { Message = response.Text, ChatId = chat.Id };
        }
    }
}