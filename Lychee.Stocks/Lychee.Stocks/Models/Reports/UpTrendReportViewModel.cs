using System.Collections.Generic;
using Lychee.Stocks.Entities;

namespace Lychee.Stocks.Models.Reports
{
    public class UpTrendReportViewModel : TrendReportBaseClass
    {
        public List<StockTrendReportModel> TwoOverTwo { get; set; }
        public List<StockTrendReportModel> ThreeOverThree { get; set; }
        public List<StockTrendReportModel> FiveOverFive { get; set; }
        public List<StockTrendReportModel> TenOverEight { get; set; }

        public List<StockTrendReportModel> TwentyOveryFifteen { get; set; }
        public List<StockTrendReportModel> ThirtyOverTwenty { get; set; }


        public override IEnumerable<List<StockTrendReportModel>> AllStockReports
        {
            get
            {
                yield return TwoOverTwo;
                yield return ThreeOverThree;
                yield return FiveOverFive;
                yield return TenOverEight;
                yield return TwentyOveryFifteen;
                yield return ThirtyOverTwenty;
            }
        }
    }
}