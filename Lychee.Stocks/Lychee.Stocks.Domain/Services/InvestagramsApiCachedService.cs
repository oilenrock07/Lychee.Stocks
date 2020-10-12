using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LazyCache;
using Lychee.Caching.Interfaces;
using Lychee.Domain.Interfaces;
using Lychee.Stocks.Domain.Constants;
using Lychee.Stocks.Domain.Interfaces.Repositories;
using Lychee.Stocks.Domain.Interfaces.Services;
using Lychee.Stocks.InvestagramsApi.Interfaces;
using Lychee.Stocks.InvestagramsApi.Models.Calendar;
using Lychee.Stocks.InvestagramsApi.Models.Social;
using Lychee.Stocks.InvestagramsApi.Models.Stocks;
using Lychee.Stocks.InvestagramsApi.Services;

namespace Lychee.Stocks.Domain.Services
{
    public class InvestagramsApiCachedService : InvestagramsApiService, IInvestagramsApiCachedService
    {
        private readonly IAppCache _cache;
        private readonly ISettingService _settingService;
        private readonly IStockMarketStatusRepository _marketStatusRepository;


        private const string CACHE_GREEN_VOLUME = "GreenVolume-{0}";
        private const string CACHE_LATEST_MARKET_ACTIVITY = "LatestStockMarketActivity-{0}";



        private IEnumerable<string> AllCacheKeys
        {
            get
            {
                var date = $"{DateTime.Now:MMdd}";
                yield return string.Format(CACHE_LATEST_MARKET_ACTIVITY, date);
                yield return string.Format(CACHE_GREEN_VOLUME, date);
            }
        }

        private readonly int _newsCacheExpiry = 20; //in minutes

        public InvestagramsApiCachedService(IStockApiRepository apiRepository, 
            ISocialApiRepository socialApiRepository,
            ICalendarApiRepository calendarApiRepository, 
            ICachingFactory cacheFactory, 
            ISettingService settingService, 
            IStockMarketStatusRepository marketStatusRepository)
            : base(apiRepository, socialApiRepository, calendarApiRepository)
        {
            _settingService = settingService;
            _marketStatusRepository = marketStatusRepository;
            _cache = cacheFactory.GetCacheService();
        }

        public override async Task<LatestStockMarketActivityVm> GetLatestStockMarketActivity()
        {
            var cacheKey = string.Format(CACHE_LATEST_MARKET_ACTIVITY, $"{DateTime.Now:MMdd}");

            //if within trading day and time, always fetch the data
            if (IsTradingHours())
            {
                var data = await base.GetLatestStockMarketActivity();

                if (_cache.Get<LatestStockMarketActivityVm>(cacheKey) != null)
                    _cache.Remove(cacheKey);

                return data;
            }

            
            return await _cache.GetOrAddAsync(cacheKey, () => base.GetLatestStockMarketActivity(), _marketStatusRepository.NextClosingDateTime());
        }

        public override async Task<ViewStock> ViewStock(string stockCode)
        {
            var cacheKey = $"ViewStock-{stockCode}-{DateTime.Now:MMdd}";

            //if within trading day and time, always fetch the data
            if (IsTradingHours())
            {
                var data = await base.ViewStock(stockCode);

                if (_cache.Get<ViewStock>(cacheKey) != null)
                    _cache.Remove(cacheKey);
                
                return data;
            }


            return await _cache.GetOrAddAsync(cacheKey, () => base.ViewStock(stockCode), _marketStatusRepository.NextClosingDateTime());
        }

        public override async Task<ViewStock> ViewStockWithoutFundamentalAnalysis(string stockCode)
        {
            var cacheKey = $"StockWithoutFundamentalAnalysis-{stockCode}-{DateTime.Now:MMdd}";

            //if within trading day and time, always fetch the data
            if (IsTradingHours())
            {
                var data = await base.ViewStockWithoutFundamentalAnalysis(stockCode);

                if (_cache.Get<ViewStock>(cacheKey) != null)
                    _cache.Remove(cacheKey);

                return data;
            }


            return await _cache.GetOrAddAsync(cacheKey, () => base.ViewStockWithoutFundamentalAnalysis(stockCode), _marketStatusRepository.NextClosingDateTime());
        }

        public override async Task<TechnicalAnalysisInfo> GetLatestTechnicalAnalysis(string stockCode)
        {
            var cacheKey = $"LatestTechnicalAnalysis-{stockCode}-{DateTime.Now:MMdd}";

            //if within trading day and time, always fetch the data
            if (IsTradingHours())
            {
                var data = await base.GetLatestTechnicalAnalysis(stockCode);

                if (_cache.Get<TechnicalAnalysisInfo>(cacheKey) != null)
                    _cache.Remove(cacheKey);

                return data;
            }


            return await _cache.GetOrAddAsync(cacheKey, () => base.GetLatestTechnicalAnalysis(stockCode), _marketStatusRepository.NextClosingDateTime());
        }

        public override async Task<AskAndBid> GetAskAndBidByStockId(int investagramStockId)
        {
            var cacheKey = $"AskAndBidByStockId-{investagramStockId}-{DateTime.Now:MMdd}";
            
            //if within trading day and time, always fetch the data
            if (IsTradingHours())
            {
                var data = await base.GetAskAndBidByStockId(investagramStockId);

                if (_cache.Get<AskAndBid>(cacheKey) != null)
                    _cache.Remove(cacheKey);

                return data;
            }


            return await _cache.GetOrAddAsync(cacheKey, () => base.GetAskAndBidByStockId(investagramStockId), _marketStatusRepository.NextClosingDateTime());
        }

        public override async Task<LatestStockHistory> GetLatestStockHistoryByStockId(int investagramStockId)
        {
            var cacheKey = $"LatestStockHistory-{investagramStockId}-{DateTime.Now:MMdd}";

            //if within trading day and time, always fetch the data
            if (IsTradingHours())
            {
                var data = await base.GetLatestStockHistoryByStockId(investagramStockId);

                if (_cache.Get<LatestStockHistory>(cacheKey) != null)
                    _cache.Remove(cacheKey);

                return data;
            }


            return await _cache.GetOrAddAsync(cacheKey, () => base.GetLatestStockHistoryByStockId(investagramStockId), _marketStatusRepository.NextClosingDateTime());
        }

        public override async Task<ChartHistory> GetChartHistoryByDate(int investagramStockId, DateTime date)
        {
            var cacheKey = $"ChartHistoryByDate-{investagramStockId}-{date:MMdd}";

            //if within trading day and time, always fetch the data
            if (IsTradingHours())
            {
                var data = await base.GetChartHistoryByDate(investagramStockId, date);

                if (_cache.Get<ChartHistory>(cacheKey) != null)
                    _cache.Remove(cacheKey);

                return data;
            }


            return await _cache.GetOrAddAsync(cacheKey, () => base.GetChartHistoryByDate(investagramStockId, date), _marketStatusRepository.NextClosingDateTime());
        }

        public override async Task<BullBearData> GetBullBearData(int investagramStockId)
        {
            var cacheKey = $"BullBearData-{investagramStockId}-{DateTime.Now:MMdd}";

            //if within trading day and time, always fetch the data
            if (IsTradingHours())
            {
                var data = await base.GetBullBearData(investagramStockId);
                SafeRemoveCache<BullBearData>(cacheKey);
                return data;
            }


            return await _cache.GetOrAddAsync(cacheKey, () => base.GetBullBearData(investagramStockId), _marketStatusRepository.NextClosingDateTime());
        }

        public override async Task<List<TrendingStock>> GetTrendingStocks()
        {
            var cacheKey = $"TrendingStocks-{DateTime.Now:MMdd}";
            return await _cache.GetOrAddAsync(cacheKey, () => base.GetTrendingStocks(), _marketStatusRepository.NextClosingDateTime());
        }

        public override async Task<MarketStatus> GetMarketStatus(DateTime date)
        {
            var cacheKey = $"MarketStatus-{DateTime.Now:MMdd}";

            //if within trading day and time, always fetch the data
            if (IsTradingHours())
            {
                var data = await base.GetMarketStatus(date);

                if (_cache.Get<MarketStatus>(cacheKey) != null)
                    _cache.Remove(cacheKey);

                return data;
            }

            return await _cache.GetOrAddAsync(cacheKey, () => base.GetMarketStatus(date), _marketStatusRepository.NextClosingDateTime());
        }


        public override async Task<CalendarOverview> GetCalendarOverview()
        {
            var cacheKey = $"CalendarOverview-{DateTime.Now:MMdd}";
            return await _cache.GetOrAddAsync(cacheKey, () => base.GetCalendarOverview(), _marketStatusRepository.NextClosingDateTime());
        }


        #region Screener
        public async Task<List<ScreenerResponse>> GetAllLatestStocks()
        {
            var cacheKey = $"AllLatestStocks-{DateTime.Now:MMdd}";
            return await GetScreenerRequest(new Screener(), cacheKey);
        }

        public async Task<List<ScreenerResponse>> GetMacdAboutToCrossFromBelowBullish()
        {
            var cacheKey = $"MacdAboutToCrossFromBelowBullish-{DateTime.Now:MMdd}";
            var request = new Screener { TechnicalMacd = 20 };
            return await GetScreenerRequest(request, cacheKey);
        }

        public async Task<List<ScreenerResponse>> GetMacdCrossingSignalFromBelowBullish()
        {
            var cacheKey = $"MacdCrossingSignalFromBelowBullish-{DateTime.Now:MMdd}";
            var request = new Screener { TechnicalMacd = 2 };
            return await GetScreenerRequest(request, cacheKey);
        }

        public async Task<List<ScreenerResponse>> Get52WeekLow()
        {
            var cacheKey = $"52WeekLow-{DateTime.Now:MMdd}";
            var request = new Screener { TechnicalWeekLow52 = 2 };
            return await GetScreenerRequest(request, cacheKey);
        }

        public async Task<List<ScreenerResponse>> GreenVolume()
        {
            var cacheKey = string.Format(CACHE_GREEN_VOLUME, $"{DateTime.Now:MMdd}");
            var request = new Screener { DescriptiveChangePercent = 2, TechnicalRelativeStrengthIndex14 = 28, TechnicalVolumeAverage20 = 2};
            return await GetScreenerRequest(request, cacheKey);
        }

        /// <summary>
        /// Oversold <= 30
        /// MA9 below SMA20
        /// </summary>
        /// <returns></returns>
        public async Task<List<ScreenerResponse>> GetOversoldStocks()
        {
            var cacheKey = $"OversoldStocks-{DateTime.Now:MMdd}";
            var request = new Screener { TechnicalMovingAverage9 = 53, TechnicalRelativeStrengthIndex14 = 8};
            return await GetScreenerRequest(request, cacheKey);
        }

        /// <summary>
        /// Oversold
        /// RSI < 10
        /// </summary>
        /// <returns></returns>
        public async Task<List<ScreenerResponse>> GetOversoldStocksLessThan20()
        {
            var cacheKey = $"OversoldStocksLessThan20-{DateTime.Now:MMdd}";
            var request = new Screener { TechnicalRelativeStrengthIndex14 = 9 };
            return await GetScreenerRequest(request, cacheKey);
        }
        #endregion


        #region News
        public override async Task<List<News>> GetFinancialReportNews(int lastStockNewsId = -1)
        {
            var cacheKey = $"FinancialReportNews-{lastStockNewsId}";
            return await _cache.GetOrAddAsync(cacheKey, () => base.GetFinancialReportNews(lastStockNewsId), TimeSpan.FromMinutes(_newsCacheExpiry));
        }

        public override async Task<List<News>> GetBusinessNews(int lastStockNewsId = -1)
        {
            var cacheKey = $"BusinessNews-{lastStockNewsId}";
            return await _cache.GetOrAddAsync(cacheKey, () => base.GetBusinessNews(lastStockNewsId), TimeSpan.FromMinutes(_newsCacheExpiry));
        }

        public override async Task<List<News>> GetDisclosureNews(int lastStockNewsId = -1)
        {
            var cacheKey = $"DisclosureNews-{lastStockNewsId}";
            return await _cache.GetOrAddAsync(cacheKey, () => base.GetDisclosureNews(lastStockNewsId), TimeSpan.FromMinutes(_newsCacheExpiry));
        }

        public override async Task<List<News>> GetNewsByStockId(int stockId, int lastStockNewsId = -1)
        {
            var cacheKey = $"NewsByStockId-{stockId}-{lastStockNewsId}";
            return await _cache.GetOrAddAsync(cacheKey, () => base.GetNewsByStockId(stockId, lastStockNewsId), TimeSpan.FromMinutes(_newsCacheExpiry));
        }
        #endregion

        private bool IsTradingHours()
        {
            var lastTradingDate = _marketStatusRepository.GetLastTradingDate();
            var openingTime = _settingService.GetSettingValue<DateTime>(SettingNames.TradingOpeningTime);
            var closingTime = _settingService.GetSettingValue<DateTime>(SettingNames.TradingClosingTime);
            var now = DateTime.Now;

            return now.Day == lastTradingDate.Day &&
                   now.TimeOfDay >= openingTime.TimeOfDay && now.TimeOfDay <= closingTime.TimeOfDay;
        }

        private void SafeRemoveCache<T>(string cacheKey)
        {
            if (_cache.Get<T>(cacheKey) != null)
                _cache.Remove(cacheKey);
        }

        public async Task<List<ScreenerResponse>> GetScreenerRequest(Screener request, string cacheKey)
        {
            //if within trading day and time, always fetch the data
            if (IsTradingHours())
            {
                var data = await base.GetScreenerResponse(request);
                SafeRemoveCache<List<ScreenerResponse>>(cacheKey);
                return data;
            }

            return await _cache.GetOrAddAsync(cacheKey, () => base.GetScreenerResponse(request), _marketStatusRepository.NextClosingDateTime());
        }

        public void ClearAllCache()
        {
            foreach (var key in AllCacheKeys)
            {
                _cache.Remove(key);
            }
        }
    }
}
