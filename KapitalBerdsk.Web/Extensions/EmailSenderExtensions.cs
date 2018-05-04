using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using KapitalBerdsk.Web.Services;
using Microsoft.AspNetCore.Http;

namespace KapitalBerdsk.Web.Services
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailInvitationAsync(this IEmailSender emailSender, string appHost, string email, string userPassword)
        {
            return emailSender.SendEmailAsync(email, "Вы были добавлены в систему",
                $"Для Вас создан аккаунт в {appHost}. Ваш временный пароль {userPassword}");
        }
    }
}
