using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LazyCache;
using Lychee.Caching.Interfaces;
using Lychee.Domain.Interfaces;
using Lychee.Stocks.Domain.Constants;
using Lychee.Stocks.Domain.Interfaces.Repositories;
using Lychee.Stocks.InvestagramsApi.Interfaces;
using Lychee.Stocks.InvestagramsApi.Models.Social;
using Lychee.Stocks.InvestagramsApi.Models.Stocks;
using Lychee.Stocks.InvestagramsApi.Services;

namespace Lychee.Stocks.Domain.Repositories
{
    public class InvestagramsApiCachedService : InvestagramsApiService
    {
        private readonly IAppCache _cache;
        private readonly ISettingService _settingService;
        private readonly IStockMarketStatusRepository _marketStatusRepository;

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
            var cacheKey = $"LatestStockMarketActivity-{DateTime.Now:MMdd}";

            //if within trading day and time, always fetch the data
            if (IsTradingHours())
            {
                var data = await base.GetLatestStockMarketActivity();

                if (_cache.Get<LatestStockMarketActivityVm>(cacheKey) != null)
                    _cache.Remove(cacheKey);

                return data;
            }

            
            return await _cache.GetOrAddAsync(cacheKey, () => base.GetLatestStockMarketActivity(), TimeSpan.FromDays(1));
        }

        public override async Task<List<TrendingStock>> GetTrendingStocks()
        {
            var cacheKey = $"TrendingStocks-{DateTime.Now:MMdd}";

            //if within trading day and time, always fetch the data
            if (IsTradingHours())
            {
                var data = await base.GetTrendingStocks();

                if (_cache.Get<List<TrendingStock>>(cacheKey) != null)
                    _cache.Remove(cacheKey);

                return data;
            }

            return await _cache.GetOrAddAsync(cacheKey, () => base.GetTrendingStocks(), TimeSpan.FromDays(1));
        }

        private bool IsTradingHours()
        {
            var lastTradingDate = _marketStatusRepository.GetLastTradingDate();
            var openingTime = _settingService.GetSettingValue<DateTime>(SettingNames.TradingOpeningTime);
            var closingTime = _settingService.GetSettingValue<DateTime>(SettingNames.TradingClosingTime);
            var now = DateTime.Now;

            return now.Day == lastTradingDate.Day &&
                   now.TimeOfDay >= openingTime.TimeOfDay && now.TimeOfDay <= closingTime.TimeOfDay;
        }
    }
}
