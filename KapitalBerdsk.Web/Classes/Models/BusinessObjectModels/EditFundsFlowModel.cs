﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KapitalBerdsk.Web.Classes.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KapitalBerdsk.Web.Classes.Models.BusinessObjectModels
{
    public class EditFundsFlowModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = Resources.ResourceKeys.Required)]
        [MaxLength(2000)]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required(ErrorMessage = Resources.ResourceKeys.Required)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата")]
        public DateTime? Date { get; set; }

        [Display(Name = "Приход")]
        public decimal? Income { get; set; }

        [Display(Name = "Расход")]
        public decimal? Outgo { get; set; }

        [Display(Name = "Нал/Безнал")]
        public FundsFlow.PaymentType PayType { get; set; }

        [Required(ErrorMessage = Resources.ResourceKeys.Required)]
        [Display(Name = "Сотрудник")]
        public int? EmployeeId { get; set; }

        [Required(ErrorMessage = Resources.ResourceKeys.Required)]
        [Display(Name = "Объект")]
        public int? BuildingObjectId { get; set; }

        public IEnumerable<SelectListItem> Employees { get; set; }

        public IEnumerable<SelectListItem> BuildingObjects { get; set; }
    }
}