namespace KapitalBerdsk.Web.Classes.Extensions
{
    public static class NumberExtensions
    {
        public static decimal ToDecimal(this decimal? value)
        {
            return value.HasValue ? (decimal) value : 0;
        }
    }
}
