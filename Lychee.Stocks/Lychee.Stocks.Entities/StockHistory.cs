﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lychee.Stocks.Common.Interfaces;

namespace Lychee.Stocks.Entities
{
    [Table("StockHistory")]
    public class StockHistory :IStock
    {
        [Key]
        public int Id { get; set; }
        public string StockCode { get; set; }
        public DateTime Date { get; set; }

        [NotMapped]
        public int Trend => Last > Open ? 2 : Last == Open ? 1 : 0;

        public decimal Last { get; set; }
        public decimal ChangePercentage { get; set; }
        public decimal Open { get; set; }
        public decimal Low { get; set; }
        public decimal High { get; set; }
        public decimal Volume { get; set; }
        public decimal Value { get; set; }
        public int Trades { get; set; }
    }
}
