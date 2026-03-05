using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UcareBackApp.Chats.Dtos.Requests;
using UcareBackApp.Chats.Dtos.Responses;
using UcareBackApp.Chats.Services.Base;

[ApiController]
[Route("api/[controller]")]
public class ChatsController : ControllerBase
{
    private readonly IChatService chatService;

    public ChatsController(IChatService chatService)
    {
        this.chatService = chatService;
    }

    [Authorize(Roles = "admin")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ChatResponse>>> GetChats()
    {
        var chats = await chatService.GetChatsAsync();
        return Ok(chats);
    }

    [Authorize(Roles = "admin")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ChatResponse>> GetChat(Guid id)
    {
        try
        {
            var chat = await chatService.GetChatByIdAsync(id);
            return Ok(chat);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<ChatMessageResponse>> CreateChat([FromBody] CreateChatRequest request)
    {
        try
        {
            var response = await chatService.CreateChatAsync(request);
            return Ok(response);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost("{chatId}/messages")]
    public async Task<ActionResult<ChatMessageResponse>> SendMessage(Guid chatId, [FromBody] SendMessageRequest request)
    {
        try
        {
            var response = await chatService.SendMessageAsync(chatId, request);
            return Ok(response);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteChat(Guid id)
    {
        try
        {
            await chatService.DeleteChatAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}
