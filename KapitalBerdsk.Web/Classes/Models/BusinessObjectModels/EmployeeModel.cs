using System.ComponentModel.DataAnnotations;

namespace KapitalBerdsk.Web.Classes.Models.BusinessObjectModels
{
    public class EmployeeModel
    {
        public int Id { get; set; }

        [Display(Name = "Оклад")]
        public decimal? Salary { get; set; }

        [Required(ErrorMessage = Resources.ResourceKeys.Required)]
        [Display(Name = "ФИО")]
        public string FullName { get; set; }

        [MaxLength(256)]
        [EmailAddress(ErrorMessage = Resources.ResourceKeys.EmailInvalid)]
        public string Email { get; set; }
    }
}
