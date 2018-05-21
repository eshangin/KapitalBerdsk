using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KapitalBerdsk.Web.Classes.Models.BusinessObjectModels.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KapitalBerdsk.Web.Classes.Models.BusinessObjectModels
{
    public class EditPdSectionModel : IModelWithOneTimeEmployeeSelection
    {
        public int Id { get; set; }

        [Required(ErrorMessage = Resources.ResourceKeys.Required)]
        [MaxLength(2000)]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Сотрудник")]
        public int? EmployeeId { get; set; }

        public bool UseOneTimeEmployee { get; set; }

        [MaxLength(70)]
        public string OneTimeEmployeeName { get; set; }

        [Required(ErrorMessage = Resources.ResourceKeys.Required)]
        [Display(Name = "Объект")]
        public int? BuildingObjectId { get; set; }

        [Required(ErrorMessage = Resources.ResourceKeys.Required)]
        [Display(Name = "Цена")]
        public decimal? Price { get; set; }

        public IEnumerable<SelectListItem> Employees { get; set; }

        public int SelectedBuildingObjectId { get; set; }

        public IEnumerable<SelectListItem> BuildingObjects { get; set; }

        public bool IsCreateMode { get; set; }
    }
}
