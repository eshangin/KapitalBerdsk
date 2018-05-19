using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Data.Enums
{
    public enum EmailStatus : byte
    {
        Pending = 1,
        InProgress = 2,
        Sent = 3
    }
}
