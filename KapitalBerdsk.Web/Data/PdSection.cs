using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Data
{
    /// <summary>
    /// Section of project documentation
    /// </summary>
    public class PdSection
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Name { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int BuildingObjectId { get; set; }
        public BuildingObject BuildingObject { get; set; }

        public decimal Price { get; set; }
    }
}
