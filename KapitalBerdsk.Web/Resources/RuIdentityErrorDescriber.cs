using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Resources
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
