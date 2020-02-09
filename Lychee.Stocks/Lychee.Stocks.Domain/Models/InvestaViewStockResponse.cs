using Lychee.Stocks.Domain.Models.Investagrams;

namespace Lychee.Stocks.Domain.Models
{
    public class InvestaViewStockResponse : InvestaBaseApiResponse
    {
        public StockInfo StockInfo { get; set; }
    }
}
