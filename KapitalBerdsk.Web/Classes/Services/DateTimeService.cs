using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime LocalDate => DateTime.UtcNow.AddHours(7).Date;
    }
}
