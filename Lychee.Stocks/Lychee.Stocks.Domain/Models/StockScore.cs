using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using Lychee.Stocks.Domain.Enums;
using Lychee.Stocks.InvestagramsApi.Models.Stocks;

namespace Lychee.Stocks.Domain.Models
{
    public class StockScore
    {
        public StockScore()
        {
            Reasons = new List<ReasonScore>();
        }

        public int StockId { get; set; }

        public ChartHistory ChartHistory { get; set; }

        public decimal TotalScore => Reasons.Sum(x => x.Score);
        public List<ReasonScore> Reasons { get; }

        public List<ReasonScore> UpTrendReasons => Reasons.Where(x => x.Trend == StockTrend.Bullish).ToList();
        public List<ReasonScore> DownTrendReasons => Reasons.Where(x => x.Trend == StockTrend.Bearish).ToList();

        public bool HasSignificantUptrendReason => UpTrendReasons.Any(x => x.IsSignificant);
        public bool HasSignificantDowntrendReason => DownTrendReasons.Any(x => x.IsSignificant);


        public void AddReason(decimal score, string reason, StockTrend trend = StockTrend.Bullish, bool isSignificant = false)
        {
            if (trend == StockTrend.Bearish && score > 0)
                score = score * -1;

            Reasons.Add(new ReasonScore {Reason = reason, Score = score, Trend = trend, IsSignificant = isSignificant});
        }

        public void AddReasons(List<ReasonScore> reasons)
        {
            Reasons.AddRange(reasons);
        }

        public void AddReasons(StockScore score)
        {
            Reasons.AddRange(score.Reasons);
        }
    }
}
