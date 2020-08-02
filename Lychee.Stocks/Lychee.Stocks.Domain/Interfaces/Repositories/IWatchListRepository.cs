using System.Collections.Generic;
using Lychee.Infrastructure.Interfaces;
using Lychee.Stocks.Entities;

namespace Lychee.Stocks.Domain.Interfaces.Repositories
{
    public interface IWatchListRepository : IRepository<WatchListGroup>
    {
        List<WatchListGroup> GetAllWatchList();
        void UpsertCache(WatchListGroup watchListGroup);

        void DeleteCache(int id);
    }
}
