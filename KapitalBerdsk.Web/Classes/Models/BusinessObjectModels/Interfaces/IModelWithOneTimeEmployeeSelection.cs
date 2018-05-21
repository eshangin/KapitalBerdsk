using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Models.BusinessObjectModels.Interfaces
{
    public interface IModelWithOneTimeEmployeeSelection
    {
        [Display(Name = "Сотрудник")]
        int? EmployeeId { get; set; }

        bool UseOneTimeEmployee { get; set; }

        string OneTimeEmployeeName { get; set; }

        IEnumerable<SelectListItem> Employees { get; set; }
    }
}
