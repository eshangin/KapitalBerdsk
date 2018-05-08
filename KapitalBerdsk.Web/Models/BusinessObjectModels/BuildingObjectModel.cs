using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Models.BusinessObjectModels
{
    public class BuildingObjectModel
    {
        public BuildingObjectModel()
        {
            Statuses = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "В работе", Value = false.ToString(), Selected = true },
                new SelectListItem() { Text = "Закрыт", Value = true.ToString() }
            };
        }

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

        [Display(Name = "Статус")]
        public bool IsClosed { get; set; }

        public IEnumerable<PdSectionModel> PdSections { get; set; }

        public IEnumerable<SelectListItem> Statuses { get; set; }
    }
}
