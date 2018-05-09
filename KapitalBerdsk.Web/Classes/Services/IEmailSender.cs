using System.Collections.Generic;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toAddress, string subject, string message);
        Task SendEmailAsync(IEnumerable<string> toAddresses, string subject, string message);
    }
}
