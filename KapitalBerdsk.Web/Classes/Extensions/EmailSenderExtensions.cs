using System.Threading.Tasks;
using KapitalBerdsk.Web.Classes.Services;

namespace KapitalBerdsk.Web.Classes.Extensions
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
