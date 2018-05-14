using System;
using System.ComponentModel.DataAnnotations;
using KapitalBerdsk.Web.Classes.Data;

namespace KapitalBerdsk.Web.Classes.Models.BusinessObjectModels
{
    public class FundsFlowListItemModel
    {
        public int Id { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Дата")]
        [DisplayFormat(DataFormatString = "{0: dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Приход")]
        public decimal? Income { get; set; }

        [Display(Name = "Расход")]
        public decimal? Outgo { get; set; }

        [Display(Name = "Сотрудник")]
        public string EmployeeName { get; set; }

        public int EmployeeId { get; set; }

        [Display(Name = "Объект")]
        public string BuildingObjectName { get; set; }

        public int BuildingObjectId { get; set; }

        [Display(Name = "Нал/Безнал")]
        public FundsFlow.PaymentType PayType { get; set; }
    }
}
