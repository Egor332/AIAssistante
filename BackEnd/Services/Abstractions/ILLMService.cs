using BackEnd.DTOs;

namespace BackEnd.Services.Abstractions
{
    public interface ILLMService
    {
        public Task<string> SendMessageAsync(ChatHistoryDto chatHistoryDto);
    }
}
