using Lychee.Stocks.InvestagramsApi.Models.Stocks;

namespace Lychee.Stocks.Models.Stocks
{
    public class GreenVolumeViewModel : ScreenerResponse
    {
        public bool IsTrending { get; set; }
        public bool IsInWatchList { get; set; }
    }
}