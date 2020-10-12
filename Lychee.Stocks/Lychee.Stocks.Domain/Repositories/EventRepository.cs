using System;
using System.Collections.Generic;
using System.Linq;
using LazyCache;
using Lychee.Caching.Interfaces;
using Lychee.Infrastructure;
using Lychee.Infrastructure.Interfaces;
using Lychee.Stocks.Domain.Interfaces.Repositories;
using Lychee.Stocks.Entities;

namespace Lychee.Stocks.Domain.Repositories
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        private readonly IAppCache _cache;
        private readonly IStockMarketStatusRepository _stockMarketStatusRepository;

        public EventRepository(IDatabaseFactory databaseFactory, ICachingFactory cacheFactory, IStockMarketStatusRepository stockMarketStatusRepository) : base(databaseFactory.GetContext())
        {
            _cache = cacheFactory.GetCacheService();
            _stockMarketStatusRepository = stockMarketStatusRepository;
        }

        public Dictionary<(string,string), Event> GetEvents(DateTime date)
        {
            var cacheKey = $"Events-{date:MMdd}";

            return _cache.GetOrAdd(cacheKey, () =>
            {
                var events = Find(x => x.Date == date).ToList();

                return events.Any() ? events.ToDictionary(x => (x.StockCode, x.EventType), x => x) : null;
            }, _stockMarketStatusRepository.NextClosingDateTime());
        }
    }
}
