using System;
using KapitalBerdsk.Web.Classes.Data.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace KapitalBerdsk.Web.Classes.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser, IAuditable
    {
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
