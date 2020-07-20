using System.Collections.Generic;
using Lychee.Stocks.Domain.Models;

namespace Lychee.Stocks.Models.Stocks
{
    public class ShouldIBuyStockViewModel
    {
        public string StockCode { get; set; }
        public string ShouldIBuyStock { get; set; }

        public decimal TotalScore { get; set; }

        public List<ReasonScore> UpTrendReasons { get; set; }
        public List<ReasonScore> DownTrendReasons { get; set; }
    }
}
