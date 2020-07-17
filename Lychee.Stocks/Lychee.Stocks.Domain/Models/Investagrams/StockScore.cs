using System.Collections.Generic;
using System.Linq;
using Lychee.Stocks.Domain.Enums;

namespace Lychee.Stocks.Domain.Models.Investagrams
{
    public class StockScore
    {
        public StockScore()
        {
            Breakdowns = new List<StockScoreBreakdown>();
        }

        public decimal TotalScore => Breakdowns.Sum(x => x.Score);

        /// <summary>
        /// This is partnered with Trend
        /// Set this to true if you have a really good indicator that the stock must be bought or sold
        /// </summary>
        public bool ImportantConfirmation { get; set; }
        public StockTrend Trend { get; set; }
        public List<StockScoreBreakdown> Breakdowns { get; set; }

        public void AddBreakdown(decimal score, string reason)
        {
            Breakdowns.Add(new StockScoreBreakdown { Reason = reason, Score = score});
        }
    }

    public class StockScoreBreakdown
    {
        public string Reason { get; set; }
        public decimal Score { get; set; }
    }
}
