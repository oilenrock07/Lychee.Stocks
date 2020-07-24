using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lychee.Stocks.InvestagramsApi.Exceptions;
using Lychee.Stocks.InvestagramsApi.Interfaces;
using Lychee.Stocks.InvestagramsApi.Models.Calendar;
using Lychee.Stocks.InvestagramsApi.Models.Social;
using Lychee.Stocks.InvestagramsApi.Models.Stocks;

namespace Lychee.Stocks.InvestagramsApi.Services
{
    public class InvestagramsApiService : IInvestagramsApiService
    {
        private readonly IStockApiRepository _stockApiRepository;
        private readonly ISocialApiRepository _socialApiRepository;
        private readonly ICalendarApiRepository _calendarApiRepository;

        private readonly string _investaCookieExpiredErrorMessage = "Please update investa cookie";

        public InvestagramsApiService(IStockApiRepository apiRepository, ISocialApiRepository socialApiRepository, ICalendarApiRepository calendarApiRepository)
        {
            _stockApiRepository = apiRepository;
            _socialApiRepository = socialApiRepository;
            _calendarApiRepository = calendarApiRepository;
        }

        public virtual async Task<LatestStockMarketActivityVm> GetLatestStockMarketActivity()
        {
            var data = await _stockApiRepository.GetLatestStockMarketActivity();
            if (data == null)
                throw new InvestagramApiException(_investaCookieExpiredErrorMessage);

            return data;
        }

        public virtual async Task<ViewStock> ViewStock(string stockCode)
        {
            var data = await _stockApiRepository.ViewStock(stockCode);
            if (data == null)
                throw new InvestagramApiException(_investaCookieExpiredErrorMessage);

            return data;
        }

        public virtual async Task<ViewStock> ViewStockWithoutFundamentalAnalysis(string stockCode)
        {
            var data = await _stockApiRepository.ViewStockWithoutFundamentalAnalysis(stockCode);
            if (data == null)
                throw new InvestagramApiException(_investaCookieExpiredErrorMessage);

            return data;
        }

        public virtual async Task<AskAndBid> GetAskAndBidByStockId(int investagramStockId)
        {
            var data = await _stockApiRepository.GetAskAndBidByStockId(investagramStockId);
            if (data == null)
                throw new InvestagramApiException(_investaCookieExpiredErrorMessage);

            return data;
        }

        public virtual async Task<MarketStatus> GetMarketStatus(DateTime date)
        {
            var data = await _stockApiRepository.GetMarketStatus(date);
            if (data == null)
                throw new InvestagramApiException(_investaCookieExpiredErrorMessage);

            return data;
        }

        public virtual async Task<LatestStockHistory> GetLatestStockHistoryByStockId(int stockId)
        {
            var data = await _stockApiRepository.GetLatestStockHistoryByStockId(stockId);
            if (data == null)
                throw new InvestagramApiException(_investaCookieExpiredErrorMessage);

            return data;
        }

        public virtual async Task<TechnicalAnalysisInfo> GetLatestTechnicalAnalysis(string stockCode)
        {
            var data = await _stockApiRepository.GetLatestTechnicalAnalysis(stockCode);
            if (data == null)
                throw new InvestagramApiException(_investaCookieExpiredErrorMessage);

            return data;
        }

        public virtual async Task<List<RealTimePrice>> GetAllActiveStockPriceRealTime()
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

        public virtual async Task<CalendarOverview> GetCalendarOverview()
        {
            var data = await _calendarApiRepository.GetCalendarOverview();
            if (data == null)
                throw new InvestagramApiException(_investaCookieExpiredErrorMessage);

            return data;
        }

        public virtual async Task<List<TrendingStock>> GetTrendingStocks()
        {
            var data = await _socialApiRepository.GetTrendingStocks();
            if (data == null)
                throw new InvestagramApiException(_investaCookieExpiredErrorMessage);

            return data;
        }

        public virtual async Task<ChartHistory> GetChartHistoryByDate(int stockId, DateTime date)
        {
            var data = await _stockApiRepository.GetChartHistoryByDate(stockId, date);
            if (data == null)
                throw new InvestagramApiException(_investaCookieExpiredErrorMessage);

            return data;
        }

        public virtual async Task<ChartByMinute> GetChartByMinutes(string stockCode, int minute)
        {
            var data = await _stockApiRepository.GetChartByMinutes(stockCode, minute);
            if (data == null)
                throw new InvestagramApiException(_investaCookieExpiredErrorMessage);

            return data;
        }

        public virtual async Task<BullBearData> GetBullBearData(int stockId)
        {
            var data = await _stockApiRepository.GetBullBearData(stockId);
            if (data == null)
                throw new InvestagramApiException(_investaCookieExpiredErrorMessage);

            return data;
        }

        public virtual async Task<List<ScreenerResponse>> GetAllLatestStocks()
        {
            var data = await _stockApiRepository.GetAllLatestStocks();
            if (data == null)
                throw new InvestagramApiException(_investaCookieExpiredErrorMessage);

            return data;
        }

        public virtual async Task<List<News>> GetDisclosureNews(int lastStockNewsId = -1)
        {
            var data = await _stockApiRepository.GetDisclosureNews(lastStockNewsId);
            if (data == null)
                throw new InvestagramApiException(_investaCookieExpiredErrorMessage);

            return data;
        }

        public virtual async Task<List<News>> GetFinancialReportNews(int lastStockNewsId = -1)
        {
            var data = await _stockApiRepository.GetFinancialReportNews(lastStockNewsId);
            if (data == null)
                throw new InvestagramApiException(_investaCookieExpiredErrorMessage);

            return data;
        }

        public virtual async Task<List<News>> GetBusinessNews(int lastStockNewsId = -1)
        {
            var data = await _stockApiRepository.GetBusinessNews(lastStockNewsId);
            if (data == null)
                throw new InvestagramApiException(_investaCookieExpiredErrorMessage);

            return data;
        }

        public virtual async Task<List<News>> GetNewsByStockId(int stockId, int lastStockNewsId = -1)
        {
            var data = await _stockApiRepository.GetNewsByStockId(stockId, lastStockNewsId);
            if (data == null)
                throw new InvestagramApiException(_investaCookieExpiredErrorMessage);

            return data;
        }

    }


}
