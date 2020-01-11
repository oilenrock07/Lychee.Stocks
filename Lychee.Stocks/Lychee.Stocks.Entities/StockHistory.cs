using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lychee.Stocks.Entities
{
    [Table("StockHistory")]
    public class StockHistory
    {
        [Key]
        public int Id { get; set; }
        public string StockCode { get; set; }
        public DateTime Date { get; set; }

        [NotMapped]
        public int Trend => Last > SOpen ? 2 : Last == SOpen ? 1 : 0;

        public decimal Last { get; set; }
        public decimal Change { get; set; }
        public decimal ChangePercentage { get; set; }
        public decimal SOpen { get; set; }
        public decimal Low { get; set; }
        public decimal High { get; set; }
        public decimal Volume { get; set; }
    }
}
