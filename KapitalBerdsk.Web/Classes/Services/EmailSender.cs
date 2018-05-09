using System.Collections.Generic;
using System.Threading.Tasks;
using KapitalBerdsk.Web.Classes.Options;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace KapitalBerdsk.Web.Classes.Services
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

        public Task SendEmailAsync(string toAddress, string subject, string message)
        {
            return SendEmailAsync(new List<string>() { toAddress }, subject, message);
        }

        public Task SendEmailAsync(IEnumerable<string> toAddresses, string subject, string message)
        {
            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress("", _smtpOptions.From));
            foreach (var to in toAddresses)
            {
                mailMessage.To.Add(new MailboxAddress("", to));
            }
            mailMessage.Subject = subject;

            mailMessage.Body = new TextPart("html")
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect(_smtpOptions.Host, _smtpOptions.Port, _smtpOptions.EnableSsl);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(_smtpOptions.UserName, _smtpOptions.Password);

                client.Send(mailMessage);
                client.Disconnect(true);
            }

            return Task.CompletedTask;
        }
    }
}
