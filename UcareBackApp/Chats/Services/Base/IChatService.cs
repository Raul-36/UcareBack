using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;
using UcareBackApp.Chats.Dtos.Requests;
using UcareBackApp.Chats.Dtos.Responses;
using UcareBackApp.Chats.Entities;

namespace UcareBackApp.Chats.Services.Base
{
    public interface IChatService
    {
        Task<IEnumerable<Dtos.Responses.ChatResponse>> GetChatsAsync();
        Task<Dtos.Responses.ChatResponse> GetChatByIdAsync(Guid chatId);
        Task<ChatMessageResponse> CreateChatAsync(CreateChatRequest request);
        Task<ChatMessageResponse> SendMessageAsync(Guid chatId, SendMessageRequest request);
        Task DeleteChatAsync(Guid chatId);
    }
}