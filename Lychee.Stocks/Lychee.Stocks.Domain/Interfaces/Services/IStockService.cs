using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lychee.Scrapper.Domain.Models.Scrappers;
using Lychee.Stocks.Entities;

namespace Lychee.Stocks.Domain.Interfaces.Services
{
    public interface IStockService
    {
        Task UpdateAllStocks();

        Task<ResultCollection<ResultItemCollection>> FetchRealTimeStocks();

        void SaveStocks(ResultCollection<ResultItemCollection> collections);

        bool HasStockData(DateTime date);

        ICollection<MyPrediction> GetLast5DaysPredictions();

        DateTime GetLastDataUpdates();

        ICollection<StockTrendReportModel> GetStockTrendReport(int days, int losingWinningStreak,
            string trend = "Bearish");
    }
}
