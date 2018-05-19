using KapitalBerdsk.Web.Classes.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Services
{
    public interface IEmailSender
    {
        Task<int> AddPendingEmail(string from, string to, string subject, string body);
        Task<int> AddPendingEmail(string to, string subject, string body);
        Task<IEnumerable<int>> AddPendingEmails(IEnumerable<Email> emails);
        Task HandlePendingEmail(int emailId);
    }
}
