using System.ComponentModel.DataAnnotations;

namespace KapitalBerdsk.Web.Classes.Models.BusinessObjectModels
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

        [Display(Name = "Подотчет")]
        public decimal AccountableBalance { get; set; }

        [Display(Name = "Начислено")]
        public decimal Accrued { get; set; }

        [Display(Name = "Остаток")]
        public decimal Balance { get; set; }
    }
}
