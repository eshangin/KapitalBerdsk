using System.ComponentModel.DataAnnotations;

namespace KapitalBerdsk.Web.Classes.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = Resources.ResourceKeys.Required)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
