using BackEnd.Models.Forms;

namespace BackEnd.Utilities
{
    public enum LLMResponseTypes{
        ValidJson,
        InvalidJson,
        NotJson
    }

    public class LLMResponseInfo
    {
        public LLMResponseTypes Type { get; set; }
        public string? Message { get; set; }
        public HelpdeskForm? Form { get; set; }
    }
}
