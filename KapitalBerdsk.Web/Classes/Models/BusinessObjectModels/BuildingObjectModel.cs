﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KapitalBerdsk.Web.Classes.Data.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KapitalBerdsk.Web.Classes.Models.BusinessObjectModels
{
    public class BuildingObjectModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = Resources.ResourceKeys.Required)]
        [MaxLength(500)]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Required(ErrorMessage = Resources.ResourceKeys.Required)]
        [Display(Name = "Стоимость (цена) контракта")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = Resources.ResourceKeys.Required)]
        [Display(Name = "Дата начала контракта")]
        [DisplayFormat(DataFormatString = "{0: dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ContractDateStart { get; set; }

        [Required(ErrorMessage = Resources.ResourceKeys.Required)]
        [Display(Name = "Дата окончания контракта")]
        [DisplayFormat(DataFormatString = "{0: dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ContractDateEnd { get; set; }

        [Display(Name = "Ответственный")]
        public int? ResponsibleEmployeeId { get; set; }

        [Display(Name = "Неактивен")]
        public bool IsInactive { get; set; }

        [Display(Name = "Статус")]
        public BuildingObjectStatus Status { get; set; }

        public IEnumerable<PdSectionModel> PdSections { get; set; }

        public IEnumerable<SelectListItem> Employees { get; set; }

        public bool IsCreateMode { get; set; }
    }
}
