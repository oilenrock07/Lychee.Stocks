using System.Collections.Generic;
using Lychee.Stocks.Entities;

namespace Lychee.Stocks.Domain.Interfaces.Services
{
    public interface IWatchListService
    {
        List<WatchListGroup> GetAllWatchList();

        void Add(WatchListGroup group);

        void Update(WatchListGroup group);
        void Delete(int id);

        void UpdateWatchList(Watchlist watchList);

        void DeleteWatchList(int groupId, int watchListId);

        void QuickAddWatchList(int groupId, string stockCode);
    }
}
