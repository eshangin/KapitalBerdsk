using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Data
{
    public class FundsFlow
    {
        public enum PaymentType
        {
            [Display(Name = "Нал")]
            Cash = 1,

            [Display(Name = "Безнал")]
            NonCash = 2
        }

        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public decimal? Income { get; set; }

        public decimal? Outgo { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int BuildingObjectId { get; set; }
        public BuildingObject BuildingObject { get; set; }

        public PaymentType PayType { get; set; }
    }
}
