using System.Collections.Generic;
using System.Linq;
using Lychee.Stocks.Entities;

namespace Lychee.Stocks.Models.Watchlist
{
    public class WatchlistViewModel
    {
        public int SelectedId { get; set; }
        public List<WatchListGroup> WatchListGroups { get; set; }

        public WatchListGroup SelectedWatchList =>
            WatchListGroups.FirstOrDefault(x => x.WatchListGroupId == SelectedId);
    }


}