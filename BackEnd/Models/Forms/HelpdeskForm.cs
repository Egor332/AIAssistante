using BackEnd.Results;
using System.Text;

namespace BackEnd.Models.Forms
{
    public class HelpdeskForm
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public int Urgency { get; set; }

        public ResultBase ValidateForm()
        {
            var successFlag = true;
            StringBuilder messageBuilder = new StringBuilder();
            if (string.IsNullOrEmpty(Name))
            {
                successFlag = false;
                messageBuilder.Append("Name is empty; ");
            }
            if (Name.Length > 20)
            {
                successFlag = false;
                messageBuilder.Append("Length of name is more then 20 chars; ");
            }
            if (string.IsNullOrEmpty(Surname))
            {
                successFlag = false;
                messageBuilder.Append("Surname is empty; ");
            }
            if (Surname.Length > 20)
            {
                successFlag = false;
                messageBuilder.Append("Length of surname is more then 20 chars; ");
            }
            if (string.IsNullOrEmpty(Email))
            {
                successFlag = false;
                messageBuilder.Append("Email is empty; ");
            }
            if (!Email.Contains("@"))
            {
                successFlag = false;
                messageBuilder.Append("Invalid email format; ");
            }
            if (string.IsNullOrEmpty(Description))
            {
                successFlag = false;
                messageBuilder.Append("Description is empty; ");
            }
            if (Description.Length > 100)
            {
                successFlag = false;
                messageBuilder.Append("Length of description is more then 100 chars; ");
            }
            if ((Urgency < 1) || (Urgency > 10))
            {
                successFlag = false;
                messageBuilder.Append("Urgency is not in range of 1 to 10; ");
            }
            return new ResultBase()
            {
                Success = successFlag,
                Message = messageBuilder.ToString()
            };

        }
    }
}
