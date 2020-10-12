using System;
using System.ComponentModel.DataAnnotations;

namespace Lychee.Stocks.Entities
{
    public class EventTypes
    {
        public const string Trending = "Trending";
        public const string SteepDown = "SteepDown";
        public const string GreenVolume = "GreenVolume";
        public const string HighestTrade = "HighestTrade";
        public const string HighestVolume = "HighestVolume";
        public const string MostActive = "MostActive";
        public const string TopWinner = "TopWinner";
        public const string TopLoser = "TopLoser";
        public const string _52WeekHigh = "52WeekHigh";
        public const string _52WeekLow = "52WeekLow";
        public const string Oversold = "Oversold";
    }

    public class Event
    {
        [Key]
        public int EventId { get; set; }

        public string EventType { get; set; }
        public DateTime Date { get; set; }
        public string StockCode { get; set; }

        public string String1 { get; set; }
        public decimal Decimal1 { get; set; }
    }
}
