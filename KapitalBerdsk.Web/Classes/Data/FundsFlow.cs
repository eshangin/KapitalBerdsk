using System;
using System.ComponentModel.DataAnnotations;
using KapitalBerdsk.Web.Classes.Data.Enums;
using KapitalBerdsk.Web.Classes.Data.Interfaces;
using KapitalBerdsk.Web.Classes.Models;

namespace KapitalBerdsk.Web.Classes.Data
{
    public class FundsFlow : IAuditable
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public decimal? Income { get; set; }

        public decimal? Outgo { get; set; }

        public OutgoType OutgoType { get; set; }

        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int? BuildingObjectId { get; set; }
        public BuildingObject BuildingObject { get; set; }

        public int? OrganizationId { get; set; }
        public Organization Organization { get; set; }

        public PaymentType PayType { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public string CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public string ModifiedById { get; set; }
        public ApplicationUser ModifiedBy { get; set; }
    }
}
