namespace Test.Core.Extensions
{
    public static class DateTimeStringExtensions
    {
        public static bool TryDateParse(this string dateStr, out (string, DateTime)? result)
        {
            var prefix = dateStr[..2];
            if (DateTime.TryParse(dateStr[2..], out var date))
            {
                result = (prefix, date);
                return true;
            }

            result = null;
            return false;
        }

        public static DateTime StartOfDay(this DateTime theDate)
        {
            return theDate.Date;
        }

        public static DateTime EndOfDay(this DateTime theDate)
        {
            return theDate.Date.AddDays(1).AddTicks(-1);
        }
    }
}
