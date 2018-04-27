using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = Resources.ResourceKeys.Required)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = Resources.ResourceKeys.Required)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня?")]
        public bool RememberMe { get; set; }
    }
}
