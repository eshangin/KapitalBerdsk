using System.ComponentModel.DataAnnotations;

namespace KapitalBerdsk.Web.Classes.Models.AccountViewModels
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
