using System.Collections.Generic;
using System.Linq;
using Lychee.Stocks.Entities;

namespace Lychee.Stocks.Models.Reports
{
    public class DownTrendReportViewModel : TrendReportBaseClass
    {
        public List<StockTrendReportModel> FiveOverFive { get; set; }
        public List<StockTrendReportModel> TenOverEight { get; set; }
        public List<StockTrendReportModel> TwentyOveryFifteen { get; set; }
        public List<StockTrendReportModel> ThirtyOverTwenty { get; set; }

        public override IEnumerable<List<StockTrendReportModel>> AllStockReports
        {
            get
            {
                yield return FiveOverFive;
                yield return TenOverEight;
                yield return TwentyOveryFifteen;
                yield return ThirtyOverTwenty;
            }
        }
    }
}