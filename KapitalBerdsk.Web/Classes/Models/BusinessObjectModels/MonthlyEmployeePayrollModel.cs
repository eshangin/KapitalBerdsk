using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Models.BusinessObjectModels
{
    public class MonthlyEmployeePayrollModel
    {
        public decimal Accured { get; set; }

        public decimal Issued { get; set; }

        public decimal Balance => Accured - Issued;

        public int Year { get; set; }

        public int Month { get; set; }
    }
}
