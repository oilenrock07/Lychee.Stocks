using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lychee.Stocks.Entities
{
    [Table("WatchList")]
    public class Watchlist
    {
        public Watchlist()
        {
            DateCreated = DateTime.Now;
        }

        [Key]
        public int WatchListId { get; set; }
        public string StockCode { get; set; }
        public int WatchListGroupId { get; set; }
        public decimal Entry { get; set; }
        public decimal Cutloss { get; set; }
        public decimal Target { get; set; }
        public string Note { get; set; }
        public bool Deleted { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
