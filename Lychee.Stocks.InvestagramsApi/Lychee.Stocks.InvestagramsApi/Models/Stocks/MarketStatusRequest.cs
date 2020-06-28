using System;

namespace Lychee.Stocks.InvestagramsApi.Models.Stocks
{
    public class MarketStatusRequest
    {
        public MarketStatusRequest()
        {
            ExchangeType = 1;
        }

        public int ExchangeType { get; set; }

        public DateTime Date { get; set; }
    }
}
