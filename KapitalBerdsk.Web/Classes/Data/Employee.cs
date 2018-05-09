﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KapitalBerdsk.Web.Classes.Data.Interfaces;

namespace KapitalBerdsk.Web.Classes.Data
{
    public class Employee : IAuditable
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(70)]
        public string FullName { get; set; }

        public decimal? Salary { get; set; }

        public List<PdSection> PdSections { get; set; }

        public List<FundsFlow> FundsFlows { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}