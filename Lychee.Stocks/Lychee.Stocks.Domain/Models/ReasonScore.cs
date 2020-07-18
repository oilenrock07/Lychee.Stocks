using Lychee.Stocks.Domain.Enums;

namespace Lychee.Stocks.Domain.Models
{
    public class ReasonScore
    {
        public string Reason { get; set; }
        public decimal Score { get; set; }
        public StockTrend Trend { get; set; }

        public bool IsSignificant { get; set; }
    }
}
