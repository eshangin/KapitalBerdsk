using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Models.BusinessObjectModels
{
    public class EmployeePayrollModel
    {
        public int Id { get; set; }

        public decimal Value { get; set; }

        public DateTime Date { get; set; }
    }
}
