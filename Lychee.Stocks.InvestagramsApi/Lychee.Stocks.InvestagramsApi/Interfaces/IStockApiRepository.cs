using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lychee.Stocks.InvestagramsApi.Models.Stocks;

namespace Lychee.Stocks.InvestagramsApi.Interfaces
{
    public interface IStockApiRepository
    {
        Task<LatestStockMarketActivityVm> GetLatestStockMarketActivity();

        Task<ViewStock> ViewStock(string stockCode);

        Task<ViewStock> ViewStockWithoutFundamentalAnalysis(string stockCode);

        Task<List<RealTimePrice>> GetAllActiveStockPriceRealTime(int page);

        Task<AskAndBid> GetAskAndBidByStockId(int investagramStockId);

        Task<MarketStatus> GetMarketStatus(DateTime date);

        Task<LatestStockHistory> GetLatestStockHistoryByStockId(int stockId);
        Task<TechnicalAnalysisInfo> GetLatestTechnicalAnalysis(string stockCode);
    }
}
