using System.Globalization;
using System.Linq;
using Lychee.CommonHelper.Extensions;

namespace Lychee.Stocks.Domain.Helpers
{
    public static class StockExtension
    {
        public static decimal ConvertToNumber(this string strNumber)
        {
            var suffix = strNumber.Last();

            if (char.IsLetter(suffix))
            {
                if (suffix == 'B')
                    return strNumber.Substring(0, strNumber.Length - 1).ToDecimal() * 1000000000;
                if (suffix == 'M')
                    return strNumber.Substring(0, strNumber.Length - 1).ToDecimal() * 1000000;
                if (suffix == 'K')
                    return strNumber.Substring(0, strNumber.Length - 1).ToDecimal() * 1000;
            }

            return strNumber.ToDecimal();
        }

        public static string ConvertToShortHand(this decimal number)
        {
            if (number >= 1000000000)
                return $"{number / 1000000000} B";
            if (number >= 1000000)
                return $"{number / 1000000} M";
            if (number >= 1000)
                return $"{number / 1000} K";

            return number.ToString(CultureInfo.InvariantCulture);

        }
    }
}
