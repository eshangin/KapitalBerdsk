using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using KapitalBerdsk.Web.Classes.Commands.Emails;
using KapitalBerdsk.Web.Classes.Data;
using KapitalBerdsk.Web.Classes.Options;
using MailKit.Net.Smtp;
using MediatR;
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
        private readonly IMediator _mediator;

        public EmailSender(
            IOptions<SmtpOptions> smtpOptions, 
            IMediator mediator)
        {
            _smtpOptions = smtpOptions.Value;
            _mediator = mediator;
        }

        public async Task<int> AddPendingEmail(string from, string to, string subject, string body)
        {
            var emails = new List<Email>()
            {
                new Email
                {
                    Body = body,
                    From = from,
                    ToCsv = to,
                    Subject = subject,
                    Status = Data.Enums.EmailStatus.Pending
                }
            };

            await AddPendingEmails(emails);

            return emails[0].Id;
        }

        public async Task<int> AddPendingEmail(string to, string subject, string body)
        {
            return await AddPendingEmail(null, to, subject, body);
        }

        public async Task<IEnumerable<int>> AddPendingEmails(IEnumerable<Email> emails)
        {
            foreach (var email in emails)
            {
                email.Status = Data.Enums.EmailStatus.Pending;
                email.From = email.From ?? _smtpOptions.From;
            }
            await _mediator.Send(new CreateEmailsCommand(emails));

            foreach (var email in emails)
            {
                BackgroundJob.Enqueue<IEmailSender>(sender => sender.HandlePendingEmail(email.Id));
            }

            return emails.Select(item => item.Id);
        }

        public async Task HandlePendingEmail(int emailId)
        {
            await _mediator.Send(new UpdateEmailCommand(emailId)
            {
                EmailStatus = Data.Enums.EmailStatus.InProgress
            });

            Email email = await _mediator.Send(new GetEmailByIdQuery(emailId));

            await SendEmailAsync(email.From, email.ToCsv.Split(","), email.Subject, email.Body);

            await _mediator.Send(new UpdateEmailCommand(emailId)
            {
                EmailStatus = Data.Enums.EmailStatus.Sent
            });
        }

        private Task SendEmailAsync(string from, string toAddress, string subject, string message)
        {
            return SendEmailAsync(from, new List<string>() { toAddress }, subject, message);
        }

        private Task SendEmailAsync(string from, IEnumerable<string> toAddresses, string subject, string message)
        {
            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress("", from));
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
