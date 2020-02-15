using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lychee.Scrapper.Domain.Models.Scrappers;
using Lychee.Stocks.Domain.Models.Investagrams;
using Lychee.Stocks.Entities;

namespace Lychee.Stocks.Domain.Interfaces.Services
{
    public interface IStockService
    {
        Task UpdateAllStocks();

        Task<ResultCollection<ResultItemCollection>> FetchRealTimeStocks();

        void SaveStocks(ResultCollection<ResultItemCollection> collections);

        bool HasStockData(DateTime date);

        DateTime GetLastDataUpdates();

        ICollection<StockTrendReportModel> GetStockTrendReport(int days, int losingWinningStreak,
            string trend = "Bearish");

        Task<ICollection<Models.Investagrams.SuspendedStock>> GetSuspendedStocks();
        Task UpdateSuspendedStocks();

        Task<ICollection<StockBlockSale>> GetBlockSaleStocks();
        Task UpdateBlockSaleStocks();

        Task UpdateStocks(IEnumerable<string> stockCodes);


    }
}
