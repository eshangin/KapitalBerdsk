using System.Collections.Generic;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Services
{
    public interface IEmailSender
    {
        Task<int> AddPendingEmail(string from, string to, string subject, string body);
        Task HandlePendingEmail(int emailId);
    }
}
