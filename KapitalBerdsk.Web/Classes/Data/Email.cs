using KapitalBerdsk.Web.Classes.Data.Enums;
using KapitalBerdsk.Web.Classes.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Data
{
    public class Email : IAuditableWithDates, IWithId
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        [MaxLength(256)]
        public string From { get; set; }

        [Required]
        public string ToCsv { get; set; }

        public EmailStatus Status { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
