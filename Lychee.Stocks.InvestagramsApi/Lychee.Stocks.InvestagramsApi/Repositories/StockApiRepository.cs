using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lychee.Stocks.InvestagramsApi.Interfaces;
using Lychee.Stocks.InvestagramsApi.Models.Stocks;
using RestSharp;

namespace Lychee.Stocks.InvestagramsApi.Repositories
{
    public class StockApiRepository : InvestagramsRestsharpBaseRepository, IStockApiRepository
    {
        private readonly string _stockApiPath = "/InvestaApi/Stock";

        public StockApiRepository(ICookieProviderService cookieProviderService) : base(cookieProviderService)
        {
        }

        public async Task<LatestStockMarketActivityVm> GetLatestStockMarketActivity()
        {
            var result = await PostToApi<LatestStockMarketActivityVm>($"{_stockApiPath}/getLatestStockMarketActivityVM?exchangeType=1", Method.POST);
            return result.Data;
        }

        public async Task<ViewStock> ViewStock(string stockCode)
        {
            var result = await PostToApi<ViewStock>($"{_stockApiPath}/viewStock?stockCode=PSE:{stockCode}", Method.POST);
            return result.Data;
        }

        public async Task<ViewStock> ViewStockWithoutFundamentalAnalysis(string stockCode)
        {
            var result = await PostToApi<ViewStock>($"{_stockApiPath}/ViewStockWithoutFa?stockCode=PSE:{stockCode}", Method.POST);
            return result.Data;
        }

        public async Task<List<RealTimePrice>> GetAllActiveStockPriceRealTime(int page)
        {
            var result = await PostToApi<List<RealTimePrice>>($"{_stockApiPath}/GetAllLatestActiveStockPriceRealTime?exchangeType=1&page={page}&sortColumn=1&sortReverse=false", Method.POST);
            return result.Data;
        }

        public async Task<AskAndBid> GetAskAndBidByStockId(int investagramStockId)
        {
            var result = await PostToApi<AskAndBid>($"{_stockApiPath}/getBidAskPairByStockId?stockId={investagramStockId}", Method.POST);
            return result.Data;
        }

        public async Task<MarketStatus> GetMarketStatus(DateTime date)
        {
            var request = new MarketStatusRequest { Date = date};
            var result = await PostToApi<MarketStatus>(request, $"{_stockApiPath}/getMarketStatus", Method.POST);
            return result.Data;
        }

        public async Task<LatestStockHistory> GetLatestStockHistoryByStockId(int stockId)
        {
            var result = await PostToApi<LatestStockHistory>($"{_stockApiPath}/GetLatestStockHistoryByStockId?stockId={stockId}", Method.POST);
            return result.Data;
        }

        public async Task<TechnicalAnalysisInfo> GetLatestTechnicalAnalysis(string stockCode)
        {
            var result = await PostToApi<TechnicalAnalysisInfo>($"{_stockApiPath}/GetLatestStockTechnicalAnalysisByStockCode?stockCode=PSE:{stockCode}", Method.POST);
            return result.Data;
        }

        public async Task<ChartHistory> GetChartHistoryByDate(int stockId, DateTime date)
        {
            var data = new
            {
                stockId,
                Date = date.ToString("yyyy/MM/dd")
            };

            var result = await PostToApi<ChartHistory>(data, $"{_stockApiPath}/getStockChartHistoryByStockIdAndDate", Method.POST);
            return result.Data;
        }

        public async Task<ChartByMinute> GetChartByMinutes(string stockCode, int minute)
        {
            var baseDate = new DateTime(1970, 1, 1);
            var to = DateTime.UtcNow - baseDate;
            var from = DateTime.UtcNow.AddMinutes(-10) - baseDate;
            var toMilliSec = (long) to.TotalMilliseconds / 1000;
            var fromMilliSec = (long)from.TotalMilliseconds / 1000;


            var result = await PostToApi<ChartByMinute>($"/InvestaApi/TradingViewChart/history?symbol=PSE:{stockCode}&resolution={minute}&from={fromMilliSec}&to={toMilliSec}", Method.POST);
            return result.Data;
        }

        public async Task<BullBearData> GetBullBearData(int stockId)
        {
            var result = await PostToApi<BullBearData>($"{_stockApiPath}/GetBullBearDataByStockId?stockId={stockId}&type=1&order=1", Method.POST);
            return result.Data;
        }

        public async Task<List<ScreenerResponse>> GetScreenerResponse(Screener data)
        {
            var result = await PostToApi<List<ScreenerResponse>>(data, $"{_stockApiPath}/FilterStocksForScreenerPlus", Method.POST);
            return result.Data;
        }

        public async Task<List<News>> GetDisclosureNews(int lastStockNewsId = -1)
        {
            return await GetNews(2, lastStockNewsId);
        }

        public async Task<List<News>> GetFinancialReportNews(int lastStockNewsId = -1)
        {
            return await GetNews(3, lastStockNewsId);
        }

        public async Task<List<News>> GetStockNews(int lastStockNewsId = -1)
        {
            return await GetNews(-1, lastStockNewsId);
        }

        public async Task<List<News>> GetBusinessNews(int lastStockNewsId = -1)
        {
            var result = await PostToApi<List<News>>($"{_stockApiPath}/GetNewBusinessNewsPost?lastStockNewsId={lastStockNewsId}", Method.POST);
            return result.Data;
        }

        public async Task<List<News>> GetNewsByStockId(int stockId, int lastStockNewsId = -1)
        {
            var getNew = lastStockNewsId > 0;
            var result = await PostToApi<List<News>>($"{_stockApiPath}/GetAllStockNewsByStockId?stockId={stockId}&&lastStockNewsId={lastStockNewsId}&getNew={getNew}", Method.POST);
            return result.Data;
        }

        private async Task<List<News>> GetNews(int sourceType, int lastStockNewsId)
        {
            var result = await PostToApi<List<News>>($"{_stockApiPath}/GetNewStockNewsPost?stockId=-1&sourceType={sourceType}&lastStockNewsId={lastStockNewsId}", Method.POST);
            return result.Data;
        }
    }


}

