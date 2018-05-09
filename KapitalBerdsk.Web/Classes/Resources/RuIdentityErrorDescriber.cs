using Microsoft.AspNetCore.Identity;

namespace KapitalBerdsk.Web.Classes.Resources
{
    public class RuIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
        {
            IdentityError identityError = new IdentityError()
            {
                Code = "DuplicateUserName",
                Description = $"Пользователь с имейлом {userName} уже зарегистрирован в системе"
            };
            return identityError;
        }

        public override IdentityError InvalidToken()
        {
            IdentityError identityError = new IdentityError()
            {
                Code = base.InvalidToken().Code,
                Description = $"Неверный токен"
            };
            return identityError;
        }
    }
}
