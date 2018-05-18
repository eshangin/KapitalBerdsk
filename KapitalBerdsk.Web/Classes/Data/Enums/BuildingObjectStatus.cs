using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Data.Enums
{
    public enum BuildingObjectStatus : byte
    {
        [Display(Name = "В работе")]
        Active = 1,

        [Display(Name = "Ожидает оплаты")]
        WaitingForPayment = 2,

        [Display(Name = "Закрыт")]
        Closed = 3
    }
}
