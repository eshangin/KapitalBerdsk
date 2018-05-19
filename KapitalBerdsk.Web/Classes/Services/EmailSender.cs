using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using KapitalBerdsk.Web.Classes.Data;
using KapitalBerdsk.Web.Classes.Options;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;

namespace KapitalBerdsk.Web.Classes.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        private readonly SmtpOptions _smtpOptions;
        private readonly ApplicationDbContext _context;

        public EmailSender(IOptions<SmtpOptions> smtpOptions, ApplicationDbContext context)
        {
            _smtpOptions = smtpOptions.Value;
            _context = context;
        }

        public async Task<int> AddPendingEmail(string from, string to, string subject, string body)
        {
            var email = new Email
            {
                Body = body,
                From = from,
                ToCsv = to,
                Subject = subject,
                Status = Data.Enums.EmailStatus.Pending
            };
            await _context.Emails.AddAsync(email);
            await _context.SaveChangesAsync();

            BackgroundJob.Enqueue<IEmailSender>(sender => sender.HandlePendingEmail(email.Id));

            return email.Id;
        }

        public async Task HandlePendingEmail(int emailId)
        {
            Email email = await _context.Emails.SingleAsync(item => item.Id == emailId);
            email.Status = Data.Enums.EmailStatus.InProgress;
            await _context.SaveChangesAsync();

            await SendEmailAsync(email.From, email.ToCsv.Split(","), email.Subject, email.Body);

            email.Status = Data.Enums.EmailStatus.Sent;
            await _context.SaveChangesAsync();
        }

        public Task SendEmailAsync(string toAddress, string subject, string message)
        {
            return SendEmailAsync(null, toAddress, subject, message);
        }

        public Task SendEmailAsync(string from, string toAddress, string subject, string message)
        {
            return SendEmailAsync(from, new List<string>() { toAddress }, subject, message);
        }

        public Task SendEmailAsync(string from, IEnumerable<string> toAddresses, string subject, string message)
        {
            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress("", from ?? _smtpOptions.From));
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
