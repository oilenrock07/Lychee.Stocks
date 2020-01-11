using System.Linq;
using Lychee.Scrapper.Domain.Helpers;

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
    }
}
