using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Models.BusinessObjectModels
{
    public class EmployeeDetailsModel
    {
        public class BuildingObjectDetail
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Total { get; set; }
        }

        public int Id { get; set; }

        [Display(Name = "Оклад")]
        public decimal Salary { get; set; }

        public string FullName { get; set; }

        public IEnumerable<BuildingObjectDetail> BuildingObjects { get; set; }
    }
}
