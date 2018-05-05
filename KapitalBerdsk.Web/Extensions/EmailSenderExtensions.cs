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
        public static Task SendEmailInvitationAsync(this IEmailSender emailSender, string appUrl, string email, string userPassword)
        {
            return emailSender.SendEmailAsync(email, "Вы были добавлены в систему",
                $"Для Вас создан аккаунт в <a href='{appUrl}'>{appUrl}</a>. Ваш временный пароль <b>{userPassword}<b>");
        }
    }
}
