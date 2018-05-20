using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Models.BusinessObjectModels
{
    public class OrganizationListItemModel
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Приход")]
        public decimal Income { get; set; }

        [Display(Name = "Расход")]
        public decimal Outgo { get; set; }

        [Display(Name = "Остаток")]
        public decimal Balance => Income - Outgo;
    }
}
