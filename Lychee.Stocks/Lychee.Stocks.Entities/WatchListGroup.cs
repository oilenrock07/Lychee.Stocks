using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lychee.Stocks.Entities
{
    [Table("WatchListGroup")]
    public class WatchListGroup
    {
        public int WatchListGroupId { get; set; }
        public string GroupName { get; set; }
        public bool Deleted { get; set; }

        public virtual ICollection<Watchlist> WatchLists { get; set; }
    }
}
