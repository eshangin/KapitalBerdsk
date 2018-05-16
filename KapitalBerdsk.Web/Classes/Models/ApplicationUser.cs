using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KapitalBerdsk.Web.Classes.Data.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace KapitalBerdsk.Web.Classes.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser, IAuditable
    {
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        [Required]
        public string CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }

        [Required]
        public string ModifiedById { get; set; }
        public ApplicationUser ModifiedBy { get; set; }

        public List<ApplicationUser> CreatedByMe { get; set; }
        public List<ApplicationUser> ModifiedByMe { get; set; }
    }
}
