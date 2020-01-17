using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lychee.Stocks.Entities
{
    [Table("Predictions")]
    public class MyPrediction
    {
        public int Id { get; set; }
        public string StockCode { get; set; }

        public string Prediction { get; set; }

        public DateTime DateCreated { get; set; }

        public bool? IsCorrect { get; set; }
        public string Comments { get; set; }

    }
}
