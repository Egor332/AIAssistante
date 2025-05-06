using BackEnd.Models.Forms;
using BackEnd.Models.Gemini;
using BackEnd.Services.Abstractions;
using BackEnd.Utilities;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks.Dataflow;

namespace BackEnd.Services.Implementations
{
    public class LLMResponseAnalyserService : ILLMResponseAnalyserService
    {
        private static readonly string _jsonStartTag = "[JSON_START]";
        private static readonly string _jsonEndTag = "[JSON_END]";

        public LLMResponseInfo AnalyzeLLMResponse(string response)
        {
            if (response.Contains("[JSON_START]") && response.Contains("[JSON_END]"))
            {
                var form = ExtractJson(response);
                var validationResult = form.ValidateForm();
                if (validationResult.Success)
                {
                    return new LLMResponseInfo()
                    {
                        Type = LLMResponseTypes.ValidJson,
                        Form = form,
                    };
                }
                else
                {
                    return new LLMResponseInfo()
                    {
                        Type = LLMResponseTypes.InvalidJson,
                        Message = validationResult.Message
                    };
                }
            }
            else
            {
                return new LLMResponseInfo()
                {
                    Type = LLMResponseTypes.NotJson,
                    Message = response
                };
            }
        }

        private HelpdeskForm ExtractJson(string response)
        {
            var startIndex = response.IndexOf(_jsonStartTag);
            var endIndex = response.IndexOf(_jsonEndTag);
            
            var json = response.Substring(startIndex + _jsonStartTag.Length, endIndex - (startIndex + _jsonStartTag.Length)).Trim();

            json = Regex.Replace(json, "```json\\s*|```", "", RegexOptions.IgnoreCase).Trim();

            var form = JsonSerializer.Deserialize<HelpdeskForm>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return form;
        }
    }
}
