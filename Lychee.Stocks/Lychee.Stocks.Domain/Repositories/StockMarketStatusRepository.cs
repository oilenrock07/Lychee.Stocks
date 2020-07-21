using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LazyCache;
using Lychee.Caching.Interfaces;
using Lychee.Stocks.Domain.Constants;
using Lychee.Stocks.Domain.Interfaces.Repositories;

namespace Lychee.Stocks.Domain.Repositories
{
    public class StockMarketStatusRepository : IStockMarketStatusRepository
    {
        private readonly IAppCache _cache;

        public StockMarketStatusRepository(ICachingFactory cacheFactory)
        {
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

                _cache.Add(CacheNames.LastTradingDateCacheKey, lastTradingDate, TimeSpan.FromDays(1));
            }


            return lastTradingDate.Date;
        }
    }
}
