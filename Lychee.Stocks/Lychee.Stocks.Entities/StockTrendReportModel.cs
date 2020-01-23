namespace Lychee.Stocks.Entities
{
    public class StockTrendReportModel
    {
        public string StockCode { get; set; }
        public string Name { get; set; }
        public string StockType { get; set; }
        public decimal Last { get; set; }
        public decimal Volume { get; set; }
        public decimal Low { get; set; }
        public decimal High { get; set; }
    }
}
