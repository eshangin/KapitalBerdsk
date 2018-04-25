using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Data
{
    public class BuildingObject
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Name { get; set; }
    }
}
