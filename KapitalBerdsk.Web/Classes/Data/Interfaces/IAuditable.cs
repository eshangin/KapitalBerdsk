using KapitalBerdsk.Web.Classes.Models;
using System;

namespace KapitalBerdsk.Web.Classes.Data.Interfaces
{
    public interface IAuditable
    {
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
        string CreatedById { get; set; }
        ApplicationUser CreatedBy { get; set; }
        string ModifiedById { get; set; }
        ApplicationUser ModifiedBy { get; set; }
    }
}
