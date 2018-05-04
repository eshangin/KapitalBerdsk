using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Models.ManageViewModels
{
    public class ManageIndexViewModel
    {
        public ChangePasswordViewModel ChangePasswordViewModel { get; set; }

        public UserInvitationViewModel UserInvitationViewModel { get; set; }

        public string StatusMessage { get; set; }
    }
}
