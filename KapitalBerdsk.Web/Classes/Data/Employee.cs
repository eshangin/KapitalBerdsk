using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KapitalBerdsk.Web.Classes.Data.Interfaces;
using KapitalBerdsk.Web.Classes.Models;

namespace KapitalBerdsk.Web.Classes.Data
{
    public class Employee : IAuditable, IOrderable
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(70)]
        public string FullName { get; set; }

        public decimal? Salary { get; set; }

        [MaxLength(256)]
        public string Email { get; set; }

        public int OrderNumber { get; set; }

        public List<PdSection> PdSections { get; set; }

        public List<FundsFlow> FundsFlows { get; set; }

        public List<EmployeePayroll> EmployeePayrolls { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public string CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public string ModifiedById { get; set; }
        public ApplicationUser ModifiedBy { get; set; }
    }
}
