using BackEnd.DTOs;
using BackEnd.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly ILLMService _LLMService;

        public MessagesController(ILLMService LLMService)
        {
            _LLMService = LLMService;
        }

        [HttpPost("chat-history")]
        public async Task<IActionResult> SendChatHistory([FromBody] ChatHistoryDto chatHistoryDto) 
        {
            try
            {
                var response = await _LLMService.SendMessageAsync(chatHistoryDto);
                return Ok(response);
            }
            catch (Exception ex) 
            {
                return BadRequest(new { Message = "Apologies, something went wrong", ErrorMessage = ex.Message });
            }
        }

    }
}
