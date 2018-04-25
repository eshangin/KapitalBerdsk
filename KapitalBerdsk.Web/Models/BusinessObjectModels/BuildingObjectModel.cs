using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Models.BusinessObjectModels
{
    public class BuildingObjectModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = Resources.ResourceKeys.Required)]
        [MaxLength(500)]
        [Display(Name = "Название")]
        public string Name { get; set; }

        public IEnumerable<PdSectionModel> PdSections { get; set; }
    }
}
