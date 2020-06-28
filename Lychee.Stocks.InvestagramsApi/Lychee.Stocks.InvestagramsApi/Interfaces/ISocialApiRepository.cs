using System.Collections.Generic;
using System.Threading.Tasks;
using Lychee.Stocks.InvestagramsApi.Models.Social;

namespace Lychee.Stocks.InvestagramsApi.Interfaces
{
    public interface ISocialApiRepository : IInvestagramsRestSharpBaseRepository
    {
        Task<List<TrendingStock>> GetTrendingStocks();
    }
}
