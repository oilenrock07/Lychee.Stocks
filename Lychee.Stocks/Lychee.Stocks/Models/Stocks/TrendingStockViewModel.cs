namespace Lychee.Stocks.Models.Stocks
{
    public class TrendingStockViewModel
    {
        public string StockCode { get; set; }
        public decimal Last { get; set; }
        public decimal Open { get; set; }
        public int Trades { get; set; }
    }
}