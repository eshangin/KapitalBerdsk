using System;

namespace KapitalBerdsk.Web.Classes.Data.Interfaces
{
    public interface IAuditableWithDates
    {
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
    }
}
