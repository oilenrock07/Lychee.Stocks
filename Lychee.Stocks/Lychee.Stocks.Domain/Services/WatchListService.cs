using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lychee.Infrastructure;
using Lychee.Infrastructure.Interfaces;
using Lychee.Stocks.Domain.Interfaces.Repositories;
using Lychee.Stocks.Domain.Interfaces.Services;
using Lychee.Stocks.Entities;
using Omu.ValueInjecter;
using Omu.ValueInjecter.Injections;

namespace Lychee.Stocks.Domain.Services
{
    public class WatchListService : Service<WatchListGroup>, IWatchListService
    {
        private readonly IWatchListRepository _watchListRepository;
        private readonly IStockService _stockService;

        public WatchListService(IWatchListRepository watchListRepository, IStockService stockService) : base(watchListRepository)
        {
            _watchListRepository = watchListRepository;
            _stockService = stockService;
        }

        public List<WatchListGroup> GetAllWatchList()
        {
            var watchList = _watchListRepository.GetAllWatchList();
            var stockPrice = _stockService.GetLatestStockHistory();

            foreach (var group in watchList)
            {
                foreach (var groupWatchList in group.WatchLists)
                {
                    var stock = stockPrice.FirstOrDefault(x => x.StockCode == groupWatchList.StockCode);
                    if (stock != null)
                    {
                        groupWatchList.Last = stock.Last;
                        groupWatchList.Open = stock.Open;
                    }
                        
                }
            }

            return watchList;
        }

        public void Add(WatchListGroup group)
        {
            base.AddAndSave(group);
            _watchListRepository.UpsertCache(group);
        }

        public void Update(WatchListGroup group)
        {
            if (group.WatchListGroupId == 0)
                throw new System.Exception("Group does not exists");

            var watchList = _watchListRepository.GetById(group.WatchListGroupId);
            if (watchList == null)
                throw new System.Exception("Group does not exists");

            _watchListRepository.Attach(watchList);
            watchList.InjectFrom(group);
            _watchListRepository.SaveChanges();
        }

        public void Delete(int id)
        {
            base.DeleteAndSave(id);
            _watchListRepository.DeleteCache(id);
        }

        public void QuickAddWatchList(int groupId, string stockCode)
        {
            var group = GetCachedWatchListGroup(groupId);
            _watchListRepository.Attach(group);
            group.WatchLists.Add(new Watchlist { StockCode = stockCode.ToUpper()});
            _watchListRepository.SaveChanges();


            _watchListRepository.UpsertCache(group);
        }

        public void UpdateWatchList(Watchlist watchList)
        {
            var group = GetCachedWatchListGroup(watchList.WatchListGroupId);
            var data = group.WatchLists.FirstOrDefault(x => x.WatchListId == watchList.WatchListId);
            if (data != null)
            {
                _watchListRepository.Attach(group);
                data.InjectFrom(new LoopInjection(new[] 
                { 
                    nameof(Watchlist.DateCreated), 
                    nameof(Watchlist.Deleted),
                    nameof(Watchlist.StockCode)
                }), watchList);
                _watchListRepository.SaveChanges();

                _watchListRepository.UpsertCache(group);
            }
        }

        public void DeleteWatchList(int groupId, int watchListId)
        {
            var group = GetCachedWatchListGroup(groupId);
            var watchList = group.WatchLists.FirstOrDefault(x => x.WatchListId == watchListId);
            if (watchList != null)
            {
                _watchListRepository.Attach(group);
                watchList.Deleted = true;
                _watchListRepository.SaveChanges();

                _watchListRepository.UpsertCache(group);
            }
        }

        private WatchListGroup GetCachedWatchListGroup(int groupId)
        {
            var cachedGroup = _watchListRepository.GetAllWatchList();
            var group = cachedGroup.FirstOrDefault(x => x.WatchListGroupId == groupId);
            if (group == null)
                throw new System.Exception("Group does not exists");

            return group;
        }
    }
}
