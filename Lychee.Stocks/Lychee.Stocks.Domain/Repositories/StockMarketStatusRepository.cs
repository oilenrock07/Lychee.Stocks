using System;
using LazyCache;
using Lychee.Caching.Interfaces;
using Lychee.Domain.Interfaces;
using Lychee.Stocks.Domain.Constants;
using Lychee.Stocks.Domain.Interfaces.Repositories;

namespace Lychee.Stocks.Domain.Repositories
{
    public class StockMarketStatusRepository : IStockMarketStatusRepository
    {
        private readonly IAppCache _cache;
        private readonly ISettingService _settingService;
        public StockMarketStatusRepository(ICachingFactory cacheFactory, ISettingService settingService)
        {
            _settingService = settingService;
            _cache = cacheFactory.GetCacheService();
        }

        public virtual DateTime GetLastTradingDate()
        {
            var lastTradingDate = _cache.Get<DateTime>(CacheNames.LastTradingDateCacheKey);
            if (lastTradingDate != DateTime.Now)
            {
                if (lastTradingDate == DateTime.MinValue)
                    lastTradingDate = DateTime.Now;

                if (lastTradingDate.DayOfWeek == DayOfWeek.Sunday)
                    lastTradingDate = lastTradingDate.AddDays(-2);
                if (lastTradingDate.DayOfWeek == DayOfWeek.Saturday)
                    lastTradingDate = lastTradingDate.AddDays(-1);

                //consider holiday

                _cache.Add(CacheNames.LastTradingDateCacheKey, lastTradingDate, NextClosingDateTime());
            }


            return lastTradingDate.Date;
        }

        public virtual DateTime NextClosingDateTime()
        {
            var date = DateTime.Now.Date;
            var closingTime = _settingService.GetSettingValue<DateTime>(SettingNames.TradingClosingTime);

            return date.AddDays(1).Add(closingTime.TimeOfDay);
        }
    }
}
