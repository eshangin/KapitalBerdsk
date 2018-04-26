using KapitalBerdsk.Web.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Models.BusinessObjectModels
{
    public class FundsFlowListItemModel
    {
        public int Id { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Дата")]
        public DateTime Date { get; set; }

        [Display(Name = "Приход")]
        public decimal? Income { get; set; }

        [Display(Name = "Расход")]
        public decimal? Outgo { get; set; }

        [Display(Name = "Сотрудник")]
        public string EmployeeName { get; set; }

        [Display(Name = "Объект")]
        public string BuildingObjectName { get; set; }

        [Display(Name = "Нал/Безнал")]
        public FundsFlow.PaymentType PayType { get; set; }
    }
}
