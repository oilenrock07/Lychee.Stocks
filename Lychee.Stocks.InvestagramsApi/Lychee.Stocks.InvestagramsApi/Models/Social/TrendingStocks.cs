using System;
using Lychee.Stocks.Common.Interfaces;

namespace Lychee.Stocks.InvestagramsApi.Models.Social
{
    public class TrendingStock : IStock
    {
        public DateTime TrendingDate { get; set; }
        public Stock Stock { get; set; }
        public int Count { get; set; }
        public string StockCode
        {
            get => Stock.StockCode;
            set { }
        }
    }

    public class Stock
    {
        public int StockId { get; set; }
        public string StockCode { get; set; }
        public string StockName { get; set; }
        public int ExchangeType { get; set; }
        public string ExchangeString { get; set; }
        public string StockCodeAndExchange { get; set; }
        public int StockType { get; set; }
        public string SectorString { get; set; }
        public string SubsectorString { get; set; }
        public string DisplayPhotoUrl { get; set; }
        public bool IsActive { get; set; }
        public int StockCategoryId { get; set; }
        public string StockCategory { get; set; }
    }

}
