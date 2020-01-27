using System.Collections.Generic;
using System.Linq;
using Lychee.Stocks.Entities;

namespace Lychee.Stocks.Models.Reports
{
    public abstract class TrendReportBaseClass
    {
        public List<string> StockCodesThatExistsForAll
        {
            get
            {
                if (AllStockReports.Any(x => x == null))
                    return new List<string>();

                var stockExistsOnAll = AllStockReports.First().Select(x => x.StockCode).ToList();
                foreach (var item in AllStockReports)
                {
                    stockExistsOnAll = stockExistsOnAll.Intersect(item.Select(x => x.StockCode)).ToList();
                }

                return stockExistsOnAll;

            }
        }

        public abstract IEnumerable<List<StockTrendReportModel>> AllStockReports { get; }
    }
}