using System;

namespace Lychee.Stocks.Domain.Models
{
    public class CandleStick
    {
        public decimal Close { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Open { get; set; }

        public bool IsRedCandle => Close < Open;

        public bool IsGreenCandle => Close > Open;

        public decimal Median => (High + Low) / 2;


        public decimal CandleFullHeight => High - Low;

        public decimal CandleBodyHeight => Math.Abs(Open - Close);

    }
}
