using KapitalBerdsk.Web.Classes.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Data.Extensions
{
    public static class EntitiesExtensions
    {
        public static void SetEmployee(this IWithOneTimeEmployee entity,
            bool useOneTimeEmployee, int? employeeId, string oneTimeEmployeeName)
        {
            if (useOneTimeEmployee)
            {
                entity.OneTimeEmployeeName = oneTimeEmployeeName;
                entity.EmployeeId = null;
            }
            else
            {
                entity.OneTimeEmployeeName = null;
                entity.EmployeeId = employeeId.Value;
            }
        }
    }
}
