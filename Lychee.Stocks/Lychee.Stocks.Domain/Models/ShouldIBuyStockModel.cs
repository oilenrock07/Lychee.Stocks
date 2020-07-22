using System.Collections.Generic;

namespace Lychee.Stocks.Domain.Models
{
    public class ShouldIBuyStockModel
    {
        public string StockCode { get; set; }
        public string ShouldIBuyStock { get; set; }

        public decimal TotalScore { get; set; }

        public List<ReasonScore> UpTrendReasons { get; set; }
        public List<ReasonScore> DownTrendReasons { get; set; }
    }
}
