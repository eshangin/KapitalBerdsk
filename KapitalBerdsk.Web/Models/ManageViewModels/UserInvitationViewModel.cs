using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Models.ManageViewModels
{
    public class UserInvitationViewModel
    {
        [Required(ErrorMessage = Resources.ResourceKeys.Required)]
        [EmailAddress(ErrorMessage = Resources.ResourceKeys.EmailInvalid)]
        public string Email { get; set; }
    }
}
