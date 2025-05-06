namespace BackEnd.Services.Abstractions
{
    public interface ILLMService
    {
        public Task<string> SendMessageAsync(string userMessage);
    }
}
