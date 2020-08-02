using System.Collections.Generic;
using System.Linq;
using Lychee.Stocks.Entities;

namespace Lychee.Stocks.Models.Watchlist
{
    public class WatchlistViewModel
    {
        public int SelectedId { get; set; }
        public List<WatchListGroup> WatchListGroups { get; set; }

        public WatchListGroup SelectedWatchList => SelectedId > 0 ? 
                WatchListGroups.FirstOrDefault(x => x.WatchListGroupId == SelectedId) : 
                null;
    }
}