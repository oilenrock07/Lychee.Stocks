using System;

namespace Lychee.Stocks.Entities
{
    public class SuspendedStock
    {
        public int Id { get; set; }
        public string StockCode { get; set; }
        public DateTime Date { get; set; }
    }
}
