using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Models.BusinessObjectModels.Interfaces
{
    public interface IModelWithOneTimeEmployeeSelection
    {
        int? EmployeeId { get; set; }

        bool UseOneTimeEmployee { get; set; }

        string OneTimeEmployeeName { get; set; }
    }
}
