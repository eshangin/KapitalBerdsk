using System;
using System.ComponentModel.DataAnnotations;
using KapitalBerdsk.Web.Classes.Data.Interfaces;
using KapitalBerdsk.Web.Classes.Models;

namespace KapitalBerdsk.Web.Classes.Data
{
    /// <summary>
    /// Section of project documentation
    /// </summary>
    public class PdSection : IAuditable, IWithOneTimeEmployee, IWithId
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Name { get; set; }

        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [MaxLength(70)]
        public string OneTimeEmployeeName { get; set; }

        public int BuildingObjectId { get; set; }
        public BuildingObject BuildingObject { get; set; }

        public decimal Price { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public string ModifiedById { get; set; }
        public ApplicationUser ModifiedBy { get; set; }
    }
}
