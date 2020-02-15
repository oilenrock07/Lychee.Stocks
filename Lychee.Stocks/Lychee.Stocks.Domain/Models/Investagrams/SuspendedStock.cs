using System;

namespace Lychee.Stocks.Domain.Models.Investagrams
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
