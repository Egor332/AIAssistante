using BackEnd.Models.Forms;

namespace BackEnd.Services.Abstractions
{
    public interface IFormSubmissionService
    {
        public Task<bool> SubmitFormAsync(HelpdeskForm form);
    }
}
