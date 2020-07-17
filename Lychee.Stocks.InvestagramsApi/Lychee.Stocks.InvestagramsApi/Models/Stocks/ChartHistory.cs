using System;
using System.Runtime.Serialization;

namespace Lychee.Stocks.InvestagramsApi.Models.Stocks
{
    public class ChartHistory
    {
        [DataMember(Name = "D")]
        public DateTime[] Dates { get; set; }

        [DataMember(Name = "O")]
        public decimal[] Opens { get; set; }

        [DataMember(Name = "H")]
        public decimal[] Highs { get; set; }

        [DataMember(Name = "L")]
        public decimal[] Lows { get; set; }

        [DataMember(Name = "C")]
        public decimal[] Changes { get; set; }

        [DataMember(Name = "V")]
        public int[] Volumes { get; set; }

        [DataMember(Name = "NF")]
        public int[] NetForeigns { get; set; }

        public int[] Rsi14 { get; set; }
    }


}
