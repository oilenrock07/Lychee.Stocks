namespace Lychee.Stocks.InvestagramsApi.Models.Stocks
{
    public class BullBearData
    {
        public int StockId { get; set; }
        public int TopBidVolume { get; set; }
        public int TopAskVolume { get; set; }
        public decimal BuyingAvePrice { get; set; }
        public decimal SellingAvePrice { get; set; }
        public int BuyingPercentage { get; set; }
        public int SellingPercentage { get; set; }
        public int Type { get; set; }
        public int Order { get; set; }
    }

}
