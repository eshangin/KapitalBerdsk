using System.ComponentModel.DataAnnotations;

namespace KapitalBerdsk.Web.Classes.Models.BusinessObjectModels
{
    public class PdSectionModel
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Цена")]
        public decimal Price { get; set; }

        public int? EmployeeId { get; set; }

        [Display(Name = "Сотрудник")]
        public string EmployeeName { get; set; }
    }
}
