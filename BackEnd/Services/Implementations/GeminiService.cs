using BackEnd.Models.Gemini;
using BackEnd.Services.Abstractions;
using System.Text.Json;
using BackEnd.DTOs;
using BackEnd.Mappers.Abstractions;

namespace BackEnd.Services.Implementations
{
    public class GeminiService : ILLMService
    {
        private readonly HttpClient _httpClient;
        private readonly string _curlWithApiKey;
        private readonly IMapper _mapper;

        public GeminiService(HttpClient httpClient, IConfiguration configuration, IMapper mapper)
        {
            _httpClient = httpClient;
            var curlBase = configuration["LLM:curl"];
            var apiKey = configuration["LLM:ApiKey"];
            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(curlBase))
            {
                throw new InvalidOperationException("Missing LLM configuration data");
            }
            _curlWithApiKey = curlBase + apiKey;
            _mapper = mapper;
        }

        public async Task<string> SendMessageAsync(ChatHistoryDto chatHistoryDto)
        {
            var request = _mapper.ChatHistoryDtoToLLMRequest(chatHistoryDto);

            var response = await _httpClient.PostAsJsonAsync(_curlWithApiKey, request);

            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            var geminiResponse = JsonSerializer.Deserialize<GeminiResponse>(responseJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return geminiResponse?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text ?? "No response";
        }
    }
}
