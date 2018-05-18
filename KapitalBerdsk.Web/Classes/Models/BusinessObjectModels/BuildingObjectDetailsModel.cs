using KapitalBerdsk.Web.Classes.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Models.BusinessObjectModels
{
    public class BuildingObjectDetailsModel
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Стоимость (цена) контракта")]
        public decimal? Price { get; set; }

        [Display(Name = "Дата начала контракта")]
        [DisplayFormat(DataFormatString = "{0: dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ContractDateStart { get; set; }

        [Display(Name = "Дата окончания контракта")]
        [DisplayFormat(DataFormatString = "{0: dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ContractDateEnd { get; set; }

        [Display(Name = "Ответственный")]
        public string ResponsibleEmployeeName { get; set; }

        [Display(Name = "Статус")]
        public BuildingObjectStatus Status { get; set; }

        public IEnumerable<PdSectionModel> PdSections { get; set; }
    }
}
