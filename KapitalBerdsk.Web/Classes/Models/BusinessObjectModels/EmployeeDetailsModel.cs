﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KapitalBerdsk.Web.Classes.Models.BusinessObjectModels
{
    public class EmployeeDetailsModel
    {
        public class BuildingObjectDetail
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Accrued { get; set; }
            public decimal Issued { get; set; }
            public decimal Balance { get; set; }
        }

        public int Id { get; set; }

        [Display(Name = "Оклад")]
        public decimal Salary { get; set; }

        public string FullName { get; set; }

        public IEnumerable<BuildingObjectDetail> BuildingObjects { get; set; }
    }
}
