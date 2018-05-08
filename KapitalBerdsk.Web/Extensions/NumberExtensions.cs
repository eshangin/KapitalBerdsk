using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Extensions
{
    public static class NumberExtensions
    {
        public static decimal ToDecimal(this decimal? value)
        {
            return value.HasValue ? (decimal) value : 0;
        }
    }
}
