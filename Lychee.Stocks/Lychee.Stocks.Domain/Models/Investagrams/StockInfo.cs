namespace Lychee.Stocks.Domain.Models.Investagrams
{
    public class StockInfo
    {
        public int StockId { get; set; }
        public string StockCode { get; set; }
        public string StockName { get; set; }
        public string SectorString { get; set; }
        public string SubsectorString { get; set; }
    }
}
