using System.Threading.Tasks;
using Lychee.Scrapper.Domain.Models.Scrappers;

namespace Lychee.Stocks.Domain.Interfaces.Services
{
    public interface IStockService
    {
        Task UpdateAllStocks();

        Task<ResultCollection<ResultItemCollection>> FetchRealTimeStocks();

        void SaveStocks(ResultCollection<ResultItemCollection> collections);
    }
}
