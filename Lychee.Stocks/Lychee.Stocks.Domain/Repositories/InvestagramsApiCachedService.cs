using System;
using System.Threading.Tasks;
using LazyCache;
using Lychee.Caching.Interfaces;
using Lychee.Domain.Interfaces;
using Lychee.Stocks.Domain.Constants;
using Lychee.Stocks.Domain.Interfaces.Repositories;
using Lychee.Stocks.InvestagramsApi.Interfaces;
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
            var lastTradingDate = _marketStatusRepository.GetLastTradingDate();
            var openingTime = _settingService.GetSettingValue(SettingNames.TradingOpeningTime, new TimeSpan(13, 30,0));
            var closingTime = _settingService.GetSettingValue(SettingNames.TradingClosingTime, new TimeSpan(17, 30, 0));
            var now = DateTime.Now;

            //if within trading day and time, always fetch the data
            if (now.Day == lastTradingDate.Day &&
                now.TimeOfDay >= openingTime && now.TimeOfDay <= closingTime)
            {
                var data = await base.GetLatestStockMarketActivity();
                if (_cache.Get<LatestStockMarketActivityVm>(cacheKey) != null)
                    _cache.Remove(cacheKey);
                return data;
            }

            
            return await _cache.GetOrAddAsync(cacheKey, () => base.GetLatestStockMarketActivity(), TimeSpan.FromDays(1));
        }
    }
}
