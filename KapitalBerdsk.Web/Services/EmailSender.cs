using KapitalBerdsk.Web.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        private readonly SmtpOptions _smtpOptions;

        public EmailSender(IOptions<SmtpOptions> smtpOptions)
        {
            _smtpOptions = smtpOptions.Value;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            using (SmtpClient client = new SmtpClient(_smtpOptions.Host))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_smtpOptions.UserName, _smtpOptions.Password);
                client.EnableSsl = _smtpOptions.EnableSsl;

                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(_smtpOptions.From);
                    mailMessage.To.Add(email);
                    mailMessage.Body = message;
                    mailMessage.Subject = subject;
                    client.Send(mailMessage);
                }
            }

            return Task.CompletedTask;
        }
    }
}
