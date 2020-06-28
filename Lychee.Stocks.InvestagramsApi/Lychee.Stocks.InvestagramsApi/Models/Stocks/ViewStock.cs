namespace Lychee.Stocks.InvestagramsApi.Models.Stocks
{
    public class ViewStock
    {
        public StockInfo StockInfo { get; set; }

        public LatestStockHistory LatestStockHistory { get; set; }

        public FundamentalAnalysisInfo StockFundamentalAnalysisInfo { get; set; }

        public TechnicalAnalysisInfo StockTechnicalAnalysisInfo { get; set; }
    }
}
