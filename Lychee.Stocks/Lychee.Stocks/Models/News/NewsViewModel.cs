namespace Lychee.Stocks.Models.News
{
    public class NewsViewModel
    {
        public string ImageUrl { get; set; }
        public string StockCode { get; set; }
        public string DateString { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public bool IsInWatchList { get; set; }
    }
}

