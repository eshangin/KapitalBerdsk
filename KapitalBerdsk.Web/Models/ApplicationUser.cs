using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KapitalBerdsk.Web.Data.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace KapitalBerdsk.Web.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser, IAuditable
    {
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
