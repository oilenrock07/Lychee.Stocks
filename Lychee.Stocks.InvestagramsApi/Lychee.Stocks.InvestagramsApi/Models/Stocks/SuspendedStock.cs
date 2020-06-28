using System;

namespace Lychee.Stocks.InvestagramsApi.Models.Stocks
{
    public class SuspendedStock
    {
        public int StockId { get; set; }
        public string StockCode { get; set; }
        public int ExchangeType { get; set; }
        public int StockType { get; set; }
        public DateTime Date { get; set; }

    }
}
