using System;
using System.ComponentModel.DataAnnotations;
using KapitalBerdsk.Web.Classes.Data.Interfaces;
using KapitalBerdsk.Web.Classes.Models;

namespace KapitalBerdsk.Web.Classes.Data.Enums
{
    public enum OutgoType : byte
    {
        [Display(Name = "Обычный")]
        Regular = 1,

        [Display(Name = "Подотчет")]
        Accountable = 2,

        [Display(Name = "Списать с подотчета")]
        WriteOffAccountable = 3
    }
}
