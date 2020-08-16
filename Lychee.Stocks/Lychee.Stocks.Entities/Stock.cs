using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lychee.Stocks.Entities
{
    [Table("Stocks")]
    public class Stock
    {
        [Key]
        public int StockId { get; set; }

        public string StockCode { get; set; }
        public string Name { get; set; }
        public string StockType { get; set; }
    }
}
