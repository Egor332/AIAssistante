using BackEnd.DTOs;
using BackEnd.Models.Gemini;

namespace BackEnd.Mappers.Abstractions
{
    public interface IMapper
    {
        public GeminiRequest ChatHistoryDtoToLLMRequest(ChatHistoryDto dto);
    }
}
