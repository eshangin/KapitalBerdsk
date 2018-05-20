using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Models.BusinessObjectModels
{
    public class MonthlyEmployeePayrollModel
    {
        public decimal Accrued { get; set; }

        public decimal Issued { get; set; }

        public decimal Balance => Accrued - Issued;

        public int Year { get; set; }

        public int Month { get; set; }
    }
}
