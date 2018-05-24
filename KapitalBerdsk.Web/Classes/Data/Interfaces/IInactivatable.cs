using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Data.Interfaces
{
    public interface IInactivatable
    {
        bool IsInactive { get; set; }
    }
}
