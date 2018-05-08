using KapitalBerdsk.Web.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Data
{
    public class BuildingObject : IAuditable
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Name { get; set; }

        public decimal Price { get; set; }

        public DateTime ContractDateStart { get; set; }

        public DateTime ContractDateEnd { get; set; }

        public bool IsClosed { get; set; }

        public List<PdSection> PdSections { get; set; }

        public List<FundsFlow> FundsFlows { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
