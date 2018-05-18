using System;
using System.ComponentModel.DataAnnotations;
using KapitalBerdsk.Web.Classes.Data.Interfaces;
using KapitalBerdsk.Web.Classes.Models;

namespace KapitalBerdsk.Web.Classes.Data.Enums
{
    public enum PaymentType
    {
        [Display(Name = "Нал")]
        Cash = 1,

        [Display(Name = "Безнал")]
        NonCash = 2
    }
}
