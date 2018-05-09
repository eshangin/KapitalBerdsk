using System.ComponentModel.DataAnnotations;

namespace KapitalBerdsk.Web.Classes.Models.ManageViewModels
{
    public class UserInvitationViewModel
    {
        [Required(ErrorMessage = Resources.ResourceKeys.Required)]
        [EmailAddress(ErrorMessage = Resources.ResourceKeys.EmailInvalid)]
        public string Email { get; set; }
    }
}
