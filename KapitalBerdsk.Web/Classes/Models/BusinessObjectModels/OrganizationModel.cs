using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Models.BusinessObjectModels
{
    public class OrganizationModel
    {
        public int Id { get; set; }

        [Display(Name = "Неактивна")]
        public bool IsInactive { get; set; }

        [Required(ErrorMessage = Resources.ResourceKeys.Required)]
        [MaxLength(500)]
        [Display(Name = "Название")]
        public string Name { get; set; }

        public bool IsCreateMode { get; set; }
    }
}
