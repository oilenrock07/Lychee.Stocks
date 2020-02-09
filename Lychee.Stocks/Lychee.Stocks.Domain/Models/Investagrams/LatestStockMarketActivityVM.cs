using System;
using System.Collections.Generic;

namespace Lychee.Stocks.Domain.Models.Investagrams
{
    public class LatestStockMarketActivityVm 
    {
        public DateTime Date { get; set; }
        public decimal Advance { get; set; }
        public decimal Decline { get; set; }
        public decimal Unchanged { get; set; }
        public decimal TradingIssues { get; set; }
        public decimal Trades { get; set; }
        public decimal BlockSaleVolume { get; set; }
        public decimal BlockSaleValue { get; set; }
        public decimal OddlotVolume { get; set; }
        public decimal OddlotValue { get; set; }
        public decimal ForeignBuying { get; set; }
        public decimal ForeignSelling { get; set; }
        public decimal NetForeign { get; set; }
        public decimal TotalVolume { get; set; }
        public decimal TotalValue { get; set; }

        public ICollection<StockBlockSale> StockBlockSaleList { get; set; }

        public ICollection<SuspendedStock> StockSuspensionList { get; set; }
    }
}
