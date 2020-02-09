using System;

namespace Lychee.Stocks.Domain.Models.Investagrams
{
    public class LatestStockHistory
    {
        public int StockId { get; set; }
        public DateTime Date { get; set; }
        public decimal Last { get; set; }
        public decimal Open { get; set; }
        public decimal Close { get; set; }
        public decimal Change { get; set; }
        public decimal ChangePercentage { get; set; }
        public decimal Low { get; set; }
        public decimal High { get; set; }
        public decimal Average { get; set; }
        public decimal Volume { get; set; }
        public decimal Value { get; set; }
        public decimal Trades { get; set; }

    }
}

