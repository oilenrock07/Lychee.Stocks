using Lychee.Stocks.Common.Interfaces;

namespace Lychee.Stocks.InvestagramsApi.Models.Stocks
{
    public class RealTimePrice : IStock
    {
        public int StockId { get; set; }
        public string StockCode { get; set; }
        public string StockCodeAndExchange { get; set; }
        public decimal Last { get; set; }
        public decimal ChangePercentage { get; set; }
        public decimal Value { get; set; }
        public int Trades { get; set; }
    }

}
