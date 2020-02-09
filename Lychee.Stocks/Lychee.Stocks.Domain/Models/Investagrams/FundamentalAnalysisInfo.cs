using System;

namespace Lychee.Stocks.Domain.Models.Investagrams
{
    public class FundamentalAnalysisInfo
    {
        public int StockId { get; set; }
        public decimal EarningsPerShare { get; set; }
        public decimal EarningsPerShareGrowth { get; set; }
        public decimal PriceEarningsRatio { get; set; }
        public decimal PriceBookValue { get; set; }
        public string QuarterYearDisplay { get; set; }
        public decimal CurrentAssets { get; set; }
        public decimal TotalAssets { get; set; }
        public decimal CurrentLiabilities { get; set; }
        public decimal TotalLiabilities { get; set; }
        public decimal RetainedEarnings { get; set; }
        public decimal StockholdersEquity { get; set; }
        public decimal BookValue { get; set; }
        public decimal OperatingRevenue { get; set; }
        public decimal OtherRevenue { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal OperatingExpense { get; set; }
        public decimal OtherExpense { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal NetIncome { get; set; }
        public decimal NetIncomeGrowth { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public decimal ReturnOnEquity { get; set; }
        public decimal FairValue { get; set; }
        public decimal DividendsPerShare { get; set; }
    }
}
