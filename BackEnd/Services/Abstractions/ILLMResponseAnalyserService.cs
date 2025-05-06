using BackEnd.Utilities;

namespace BackEnd.Services.Abstractions
{
    public interface ILLMResponseAnalyserService
    {
        public LLMResponseInfo AnalyzeLLMResponse(string response);        
    }
}
