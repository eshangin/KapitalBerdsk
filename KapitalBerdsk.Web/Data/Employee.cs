using KapitalBerdsk.Web.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Data
{
    public class Employee : IAuditable
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(70)]
        public string FullName { get; set; }

        public decimal Salary { get; set; }

        public List<PdSection> PdSections { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
