using System.Runtime.Serialization;

namespace Lychee.Stocks.InvestagramsApi.Models.Stocks
{
    public class ChartByMinute
    {
        [DataMember(Name = "s")]
        public string Status { get; set; }

        [DataMember(Name = "t")]
        public long[] Time { get; set; }

        [DataMember(Name = "o")]
        public decimal[] Open { get; set; }

        [DataMember(Name = "l")]
        public decimal[] Last { get; set; }

        [DataMember(Name = "h")]
        public decimal[] High { get; set; }

        [DataMember(Name = "c")]
        public decimal[] Change { get; set; }

        [DataMember(Name = "v")]
        public long[] Volume { get; set; }

    }

}
