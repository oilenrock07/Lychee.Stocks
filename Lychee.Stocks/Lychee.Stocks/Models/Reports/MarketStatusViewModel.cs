namespace Lychee.Stocks.Models.Reports
{
    public class MarketStatusViewModel
    {
        public MarketStatusItemViewModel[] MostActive { get; set; }
        public MarketStatusItemViewModel[] TopGainer { get; set; }
        public MarketStatusItemViewModel[] TopLoser { get; set; }
    }

    public class MarketStatusItemViewModel
    {
        public int StockId { get; set; }
        public string StockCode { get; set; }
        public string StockCodeAndExchange { get; set; }
        public string StockName { get; set; }
        public string DisplayPhotoUrl { get; set; }
        public decimal Last { get; set; }
        public decimal ChangePercentage { get; set; }
        public decimal Volume { get; set; }
        public decimal Value { get; set; }

        public string Badge { get; set; }
        public string BadgeClass { get; set; }
    }
}