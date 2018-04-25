﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Models.BusinessObjectModels
{
    public class EditPdSectionModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = Resources.ResourceKeys.Required)]
        [MaxLength(2000)]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Required(ErrorMessage = Resources.ResourceKeys.Required)]
        [Display(Name = "Сотрудник")]
        public int? EmployeeId { get; set; }

        [Required(ErrorMessage = Resources.ResourceKeys.Required)]
        [Display(Name = "Объект")]
        public int? BuildingObjectId { get; set; }

        [Required(ErrorMessage = Resources.ResourceKeys.Required)]
        [Display(Name = "Цена")]
        public decimal? Price { get; set; }

        public IEnumerable<SelectListItem> Employees { get; set; }

        public int SelectedBuildingObjectId { get; set; }

        public IEnumerable<SelectListItem> BuildingObjects { get; set; }
    }
}
