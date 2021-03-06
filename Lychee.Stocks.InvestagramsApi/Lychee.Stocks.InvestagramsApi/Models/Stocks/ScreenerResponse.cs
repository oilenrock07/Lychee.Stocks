﻿using Lychee.Stocks.Common.Interfaces;
using Lychee.Stocks.InvestagramsApi.Interfaces;

namespace Lychee.Stocks.InvestagramsApi.Models.Stocks
{
    public class ScreenerResponse : IStock
    {
        public int StockId { get; set; }
        public string StockCode { get; set; }
        public string StockCodeAndExchange { get; set; }
        public decimal Last { get; set; }
        public decimal ChangePercentage { get; set; }
        public decimal Open { get; set; }
        public decimal Low { get; set; }
        public decimal High { get; set; }
        public decimal Volume { get; set; }
        public decimal Value { get; set; }
        public decimal? YTD { get; set; }
        public decimal? MTD { get; set; }
        public decimal? WTD { get; set; }
        public decimal? WeekHigh52 { get; set; }
        public decimal? WeekLow52 { get; set; }
    }

}
