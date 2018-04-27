using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Models.BusinessObjectModels
{
    public class EmployeeListItemModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = Resources.ResourceKeys.Required)]
        [Display(Name = "ФИО")]
        public string FullName { get; set; }

        [Required(ErrorMessage = Resources.ResourceKeys.Required)]
        [Display(Name = "Оклад")]
        public decimal Salary { get; set; }

        [Display(Name = "Начислено")]
        public decimal Accrued { get; set; }

        [Display(Name = "Остаток")]
        public decimal Balance { get; set; }
    }
}
