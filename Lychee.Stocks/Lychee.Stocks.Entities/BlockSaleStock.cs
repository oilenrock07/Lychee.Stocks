using System;
using System.ComponentModel.DataAnnotations;

namespace Lychee.Stocks.Entities
{
    public class BlockSaleStock
    {
        [Key]
        public int Id { get; set; }

        public string StockCode { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public decimal Volume { get; set; }
        public decimal Value { get; set; }
    }
}
