using Lychee.Stocks.Domain.Models;
using Lychee.Stocks.InvestagramsApi.Models.Stocks;

namespace Lychee.Stocks.Domain.Interfaces.Services
{
    public interface ICandleStickAnalyzerService
    {
        bool IsMorningStarDoji(ChartHistory history);
        bool IsEveningStarDoji(ChartHistory history);
        bool IsDoji(CandleStick candle);

        bool IsHammer(ChartHistory history);
    }
}
