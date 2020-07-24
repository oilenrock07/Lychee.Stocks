using System;

namespace Lychee.Stocks.InvestagramsApi.Models.Stocks
{
    public class News
    {
        public int StockNewsId { get; set; }
        public string Region { get; set; }
        public DateTime Date { get; set; }
        public string DateString { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public string SourceId { get; set; }
        public int StockNewsSourceType { get; set; }
        public StockInfoVM StockInfoVM { get; set; }
    }

    public class StockInfoVM
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
