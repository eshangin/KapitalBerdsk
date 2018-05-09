using System.ComponentModel.DataAnnotations;

namespace KapitalBerdsk.Web.Classes.Models.AccountViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = Resources.ResourceKeys.Required)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = Resources.ResourceKeys.Required)]
        [StringLength(15, ErrorMessage = "{0} должен содержать от {2} до {1} символов", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Повторите новый пароль")]
        [Compare("Password", ErrorMessage = "Пароль и его подтверждение не совпадают.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
