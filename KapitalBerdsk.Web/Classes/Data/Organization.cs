using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KapitalBerdsk.Web.Classes.Data.Interfaces;

namespace KapitalBerdsk.Web.Classes.Data
{
    public class Organization : IAuditable
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Name { get; set; }

        public List<FundsFlow> FundsFlows { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
