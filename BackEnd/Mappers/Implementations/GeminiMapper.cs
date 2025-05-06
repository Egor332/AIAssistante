using BackEnd.DTOs;
using BackEnd.Mappers.Abstractions;
using BackEnd.Models.Gemini;

namespace BackEnd.Mappers.Implementations
{
    public class GeminiMapper : IMapper
    {
        private static readonly MessageDto _introductionMessage = new MessageDto
        {
            Role = "user",
            Text = "You are a helpdesk‐form assistant. You will ask the user—one at a time—for:\r\n" +
           "1) Name (max 20 chars), \r\n" +
           "2) Surname (max 20 chars),\r\n" +
           "3) Email (valid format),\r\n" +
           "4) Problem description (max 100 chars),\r\n" +
           "5) Urgency (integer 1–10).\r\n" +
           "After collecting all and verifying it with the user, respond with JSON:\r\n{ name, surname, email, description, urgency }. Respond with **only pure JSON** between `[JSON_START]` and `[JSON_END]` markers.  \r\n> " +
            "**Do not wrap JSON in triple backticks.**  \r\n> " +
            "**Do not include markdown or explanations.** "
        };

        public GeminiRequest ChatHistoryDtoToLLMRequest(ChatHistoryDto dto)
        {
            var geminiRequest = new GeminiRequest() { Contents = new List<GeminiContent>() };
            geminiRequest.Contents.Add(MessageDtoToGeminiContent(_introductionMessage));
            foreach (var message in dto.History)
            {
                geminiRequest.Contents.Add(MessageDtoToGeminiContent(message));
            }
            return geminiRequest;
        }

        private GeminiContent MessageDtoToGeminiContent(MessageDto messageDto) 
        {
            if ((messageDto.Role != "user") && (messageDto.Role != "model"))
            {
                throw new ArgumentException($"Wrong role provided: expected \"user\" or \"model\", got \"{messageDto.Role}\"");
            }
            return new GeminiContent
            {
                Role = messageDto.Role,
                Parts = new List<GeminiPart>(){ new GeminiPart() { Text = messageDto.Text } }
            };
        }
    }
}
