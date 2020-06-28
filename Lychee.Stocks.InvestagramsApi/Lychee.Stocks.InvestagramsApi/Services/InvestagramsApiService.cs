using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lychee.Stocks.InvestagramsApi.Interfaces;
using Lychee.Stocks.InvestagramsApi.Models.Calendar;
using Lychee.Stocks.InvestagramsApi.Models.Investagrams;
using Lychee.Stocks.InvestagramsApi.Models.Social;
using Lychee.Stocks.InvestagramsApi.Models.Stocks;

namespace Lychee.Stocks.InvestagramsApi.Services
{
    public class InvestagramsApiService : IInvestagramsApiService
    {
        private readonly IStockApiRepository _stockApiRepository;
        private readonly ISocialApiRepository _socialApiRepository;
        private readonly ICalendarApiRepository _calendarApiRepository;

        public InvestagramsApiService(IStockApiRepository apiRepository, ISocialApiRepository socialApiRepository, ICalendarApiRepository calendarApiRepository)
        {
            _stockApiRepository = apiRepository;
            _socialApiRepository = socialApiRepository;
            _calendarApiRepository = calendarApiRepository;
        }

        public async Task<LatestStockMarketActivityVm> GetLatestStockMarketActivity()
        {
            return await _stockApiRepository.GetLatestStockMarketActivity();
        }

        public async Task<ViewStock> ViewStock(string stockCode)
        {
            return await _stockApiRepository.ViewStock(stockCode);
        }

        public async Task<ViewStock> ViewStockWithoutFundamentalAnalysis(string stockCode)
        {
            return await _stockApiRepository.ViewStockWithoutFundamentalAnalysis(stockCode);
        }

        public async Task<AskAndBid> GetAskAndBidByStockId(int investagramStockId)
        {
            return await _stockApiRepository.GetAskAndBidByStockId(investagramStockId);
        }

        public async Task<MarketStatus> GetMarketStatus(DateTime date)
        {
            return await _stockApiRepository.GetMarketStatus(date);
        }

        public async Task<LatestStockHistory> GetLatestStockHistoryByStockId(int stockId)
        {
            return await _stockApiRepository.GetLatestStockHistoryByStockId(stockId);
        }

        public async Task<TechnicalAnalysisInfo> GetLatestTechnicalAnalysis(string stockCode)
        {
            return await _stockApiRepository.GetLatestTechnicalAnalysis(stockCode);
        }

        public async Task<List<RealTimePrice>> GetAllActiveStockPriceRealTime()
        {
            var result = new List<RealTimePrice>();
            List<RealTimePrice> prices;
            var page = 1;
            do
            {
                prices = await _stockApiRepository.GetAllActiveStockPriceRealTime(page);
                result.AddRange(prices);
                page++;
            } while (prices.Any());

            return result;
        }

        public async Task<CalendarOverview> GetCalendarOverview()
        {
            return await _calendarApiRepository.GetCalendarOverview();
        }

        public async Task<List<TrendingStock>> GetTrendingStocks()
        {
            return await _socialApiRepository.GetTrendingStocks();
        }
    }


}
