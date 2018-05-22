using KapitalBerdsk.Web.Classes.Data.Interfaces;
using KapitalBerdsk.Web.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Data
{
    public class EmployeePayroll : IAuditable, IWithId
    {
        public int Id { get; set; }

        public decimal Value { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public string ModifiedById { get; set; }
        public ApplicationUser ModifiedBy { get; set; }
    }
}
