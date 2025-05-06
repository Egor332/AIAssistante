using BackEnd.Models.Forms;
using BackEnd.Services.Abstractions;

namespace BackEnd.Services.Implementations
{
    public class FormSubmissionService : IFormSubmissionService
    {
        public async Task<bool> SubmitFormAsync(HelpdeskForm form)
        {
            await Task.Delay(1); // There should be some submission logic
            return true;
        }
    }
}
