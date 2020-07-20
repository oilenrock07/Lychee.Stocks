using System.Threading.Tasks;
using Lychee.Stocks.Domain.Models;
using Lychee.Stocks.InvestagramsApi.Models.Stocks;

namespace Lychee.Stocks.Domain.Interfaces.Services
{
    public interface IStockScoreService
    {
        Task<StockScore> GetTrendingStockScore(string stockCode);
        StockScore GetBreakingResistanceScore(ViewStock viewStock);
        StockScore GetBreakingSupport2Score(ViewStock viewStock);
        StockScore GetTradeScore(ViewStock viewStock);
        StockScore GetRsiScore(ViewStock viewStock);
        Task<StockScore> GetBidAndAskScore(ViewStock stock);
        Task<StockScore> GetMostActiveAndGainerScore(string stockCode);

        Task<StockScore> GetRecentlySuspendedAndBlockSaleScore(string stockCode);

        StockScore GetVolume15Score(ViewStock viewStock);

        StockScore GetMa9Score(ViewStock viewStock);
        StockScore GetMa20Score(ViewStock viewStock);

        Task<StockScore> GetDividendScore(string stockCode);

        decimal GetBuyStockPassingScore();
    }
}
