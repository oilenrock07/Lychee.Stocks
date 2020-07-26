using System.Collections.Generic;
using Lychee.Stocks.InvestagramsApi.Models.Stocks;

namespace Lychee.Stocks.Domain.Models
{
    public class StockAnalysisModel
    {
        public int StockId { get; set; }
        public string StockCode { get; set; }
        public string ShouldIBuyStock { get; set; }

        public decimal TotalScore { get; set; }

        public List<ReasonScore> UpTrendReasons { get; set; }
        public List<ReasonScore> DownTrendReasons { get; set; }

        public string ChartHistory { get; set; }
    }
}
