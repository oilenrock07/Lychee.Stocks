using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lychee.Stocks.Entities
{
    [Table("Stocks")]
    public class Stock
    {
        [Key]
        public string StockCode { get; set; }
        public string Name { get; set; }
        public string StockType { get; set; }
    }
}
