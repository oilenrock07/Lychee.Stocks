using System;

namespace Lychee.Stocks.InvestagramsApi.Models.Investagrams
{
    public class StockBlockSale
    {
        public int StockId { get; set; }
        public string StockCode { get; set; }
        public int ExchangeType { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public decimal Volume { get; set; }
        public decimal Value { get; set; }
    }
}
