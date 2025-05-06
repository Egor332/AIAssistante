using BackEnd.Models.Gemini;
using BackEnd.Services.Abstractions;
using System.Text.Json;
using System.Text;
using System;

namespace BackEnd.Services.Implementations
{
    public class GeminiService : ILLMService
    {
        private readonly HttpClient _httpClient;
        private readonly string _curlWithApiKey;

        public GeminiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            var curlBase = configuration["LLM:curl"];
            var apiKey = configuration["LLM:ApiKey"];
            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(curlBase))
            {
                throw new InvalidOperationException("Missing LLM configuration data");
            }
            _curlWithApiKey = curlBase + apiKey;
        }

        public async Task<string> SendMessageAsync(string userMessage)
        {
            var request = new GeminiRequest
            {
                Contents = new List<GeminiContent>
            {
                new GeminiContent
                {
                    Role = "user",
                    Parts = new List<GeminiPart> { new GeminiPart { Text = userMessage } }
                }
                }
            };

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
