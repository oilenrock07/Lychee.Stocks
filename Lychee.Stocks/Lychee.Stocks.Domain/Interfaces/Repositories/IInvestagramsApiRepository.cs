using System.Threading.Tasks;
using Lychee.Stocks.Domain.Models.Investagrams;

namespace Lychee.Stocks.Domain.Interfaces.Repositories
{
    public interface IInvestagramsApiRepository
    {
        Task<LatestStockMarketActivityVm> GetLatestStockMarketActivity();

        Task<ViewStock> ViewStock(string stockCode);
    }
}
