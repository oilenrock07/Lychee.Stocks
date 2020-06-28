namespace Lychee.Stocks.InvestagramsApi.Models.Stocks
{
    public class MarketStatus
    {
        public int ExchangeType { get; set; }
        public MostActive[] MostActive { get; set; }
        public TopGainer[] TopGainer { get; set; }
        public TopLoser[] TopLoser { get; set; }
        public object LeastActive { get; set; }
    }

    public class MostActive
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

    public class TopGainer
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

    public class TopLoser
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
