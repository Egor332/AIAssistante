using BackEnd.DTOs;
using BackEnd.Mappers.Abstractions;
using BackEnd.Models.Gemini;
using BackEnd.Services.Abstractions;
using BackEnd.Utilities;
using System.Text.Json;

namespace BackEnd.Services.Implementations
{
    public class GeminiService : ILLMService
    {
        private readonly HttpClient _httpClient;
        private readonly string _curlWithApiKey;
        private readonly IMapper _mapper;
        private ILLMResponseAnalyserService _responseAnalyserService;
        private readonly IFormSubmissionService _formSubmissionService;

        public GeminiService(HttpClient httpClient, IConfiguration configuration, IMapper mapper, 
            ILLMResponseAnalyserService responseAnalyserService, IFormSubmissionService formSubmissionService)
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
            _responseAnalyserService = responseAnalyserService;
            _formSubmissionService = formSubmissionService;
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

            var responseString =  geminiResponse?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text ?? "No response";
            
            var responseInfo = _responseAnalyserService.AnalyzeLLMResponse(responseString);

            return await InterpretResponseInfoAsync(responseInfo, chatHistoryDto);
        }

        private async Task<string> InterpretResponseInfoAsync(LLMResponseInfo responseInfo, ChatHistoryDto chatHistoryDto)
        {
            if (responseInfo.Type == LLMResponseTypes.ValidJson)
            {
                await _formSubmissionService.SubmitFormAsync(responseInfo.Form!);
                return "Form submitted!";
            }
            else if (responseInfo.Type == LLMResponseTypes.InvalidJson)
            {
                chatHistoryDto.History.Add(new MessageDto() { Role = "user", Text = responseInfo.Message! });
                return await SendMessageAsync(chatHistoryDto);
            }
            else
            {
                return responseInfo.Message!;
            }
        }
    }
}
