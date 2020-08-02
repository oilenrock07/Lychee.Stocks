using System.Collections.Generic;
using System.Linq;
using LazyCache;
using Lychee.Caching.Interfaces;
using Lychee.Infrastructure;
using Lychee.Infrastructure.Interfaces;
using Lychee.Stocks.Domain.Interfaces.Repositories;
using Lychee.Stocks.Entities;
using Omu.ValueInjecter;

namespace Lychee.Stocks.Domain.Repositories
{
    public class WatchListRepository : Repository<WatchListGroup>, IWatchListRepository
    {
        private readonly IAppCache _cache;

        public WatchListRepository(IDatabaseFactory databaseFactory, ICachingFactory cacheFactory) : base(databaseFactory.GetContext())
        {
            _cache = cacheFactory.GetCacheService();
        }

        public List<WatchListGroup> GetAllWatchList()
        {
            //this currently returns all the watchlist including the deleted.
            //There is a EF extension to IncludeFilter but it generates a yucky sql code
            var cachedGroupData = _cache.GetOrAdd("AllWatchListKey", () => Find(x => !x.Deleted).ToList());
            foreach (var watchListGroup in cachedGroupData)
            {
                watchListGroup.WatchLists = watchListGroup.WatchLists.Where(x => !x.Deleted).ToList();
            }

            return cachedGroupData;
        }

        public void UpsertCache(WatchListGroup watchListGroup)
        {
            var cachedData = GetAllWatchList();
            var group = cachedData.FirstOrDefault(x => x.WatchListGroupId == watchListGroup.WatchListGroupId);

            if (group == null)
                cachedData.Add(watchListGroup);
            else
                group.InjectFrom(watchListGroup);
        }

        public void DeleteCache(int id)
        {
            var cachedData = GetAllWatchList();
            var group = cachedData.FirstOrDefault(x => x.WatchListGroupId == id);

            if (group != null)
                cachedData.Remove(group);
        }
    }
}
