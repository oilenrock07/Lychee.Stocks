using System;

namespace Lychee.Stocks.Entities
{
    public class TechnicalAnalysis
    {
        public int Id { get; set; }
        public string StockCode { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal Support1 { get; set; }
        public decimal Support2 { get; set; }
        public decimal Resistance1 { get; set; }
        public decimal Resistance2 { get; set; }
        public decimal MA20 { get; set; }
        public decimal MA50 { get; set; }
        public decimal MA100 { get; set; }
        public decimal MA200 { get; set; }
        public decimal RSI14 { get; set; }
    }
}
