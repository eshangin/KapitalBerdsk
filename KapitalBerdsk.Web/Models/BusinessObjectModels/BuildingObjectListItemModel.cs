using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Models.BusinessObjectModels
{
    public class BuildingObjectListItemModel
    {
        public int Id { get; set; }

        [MaxLength(500)]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Стоимость (цена) контракта")]
        public decimal? Price { get; set; }

        [Display(Name = "Начало контракта")]
        [DisplayFormat(DataFormatString = "{0: dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ContractDateStart { get; set; }

        [Display(Name = "Окончание контракта")]
        [DisplayFormat(DataFormatString = "{0: dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ContractDateEnd { get; set; }

        [Display(Name = "Себестоимость")]
        public decimal CostPrice { get; set; }

        [Display(Name = "Рельная стоимость")]
        public decimal RealPrice { get; set; }

        [Display(Name = "Выплачено заказчиком")]
        public decimal PaidByCustomer { get; set; }

        [Display(Name = "Статус")]
        public bool IsClosed { get; set; }
    }
}
