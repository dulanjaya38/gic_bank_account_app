using System.Globalization;

namespace GIC.BANKACCOUNT.COMMON
{
    public static class ValidationHelper
    {
        public static DateTime? ParseDateTime(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            if (!DateTime.TryParseExact(value, "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out var result))
                return null;

            return result;
        }

        public static decimal ParseDecimal(string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;

            if (!decimal.TryParse(value, out var result))
                return 0;

            return result;
        }

        public static int ParseInt(string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;

            if (!int.TryParse(value, out var result))
                return 0;

            return result;
        }
    }
}
