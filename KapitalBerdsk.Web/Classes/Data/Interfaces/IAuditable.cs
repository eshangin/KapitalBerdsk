using System;

namespace KapitalBerdsk.Web.Classes.Data.Interfaces
{
    public interface IAuditable
    {
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
    }
}
