using KapitalBerdsk.Web.Classes.Models;
using System;

namespace KapitalBerdsk.Web.Classes.Data.Interfaces
{
    public interface IAuditable : IAuditableWithDates
    {
        string CreatedById { get; set; }
        ApplicationUser CreatedBy { get; set; }
        string ModifiedById { get; set; }
        ApplicationUser ModifiedBy { get; set; }
    }
}
