using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;

namespace UcareBackApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewApiController : ControllerBase
    {
        readonly IChatClient chatClient;
        public NewApiController(IChatClient chatClient)
        {
            this.chatClient = chatClient;
        }
        [HttpGet("test-chat")]
        public async Task<IActionResult> TestChat()
        {            
            var response = await chatClient.GetResponseAsync(new[] {
                new ChatMessage(ChatRole.System, "You are a helpful assistant."),
                new ChatMessage(ChatRole.User, "Сколько будет 2+2?")
            });

            Console.WriteLine("Chat response: " + response.ModelId);
            return Ok(new { response.Messages });
        }
    }
}