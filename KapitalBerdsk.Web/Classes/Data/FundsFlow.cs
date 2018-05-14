using System;
using System.ComponentModel.DataAnnotations;
using KapitalBerdsk.Web.Classes.Data.Interfaces;

namespace KapitalBerdsk.Web.Classes.Data
{
    public class FundsFlow : IAuditable
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

        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int BuildingObjectId { get; set; }
        public BuildingObject BuildingObject { get; set; }

        public int? OrganizationId { get; set; }
        public Organization Organization { get; set; }

        public PaymentType PayType { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
