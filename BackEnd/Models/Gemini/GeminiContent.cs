namespace BackEnd.Models.Gemini
{
    public class GeminiContent
    {
        public string Role { get; set; }
        public List<GeminiPart> Parts { get; set; }
    }
}
