using Lychee.Stocks.Common.Interfaces;

namespace Lychee.Stocks.InvestagramsApi.Models.Stocks
{
    public class MarketStatus
    {
        public int ExchangeType { get; set; }
        public MarketStatusModel[] MostActive { get; set; }
        public MarketStatusModel[] TopGainer { get; set; }
        public MarketStatusModel[] TopLoser { get; set; }
        public object LeastActive { get; set; }
    }

    public class MarketStatusModel : IStock
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
    }
}
