﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lychee.Scrapper.Domain.Models.Scrappers;
using Lychee.Stocks.Entities;
using Lychee.Stocks.InvestagramsApi.Models.Stocks;

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

        Task<ICollection<InvestagramsApi.Models.Stocks.SuspendedStock>> GetSuspendedStocks();
        Task UpdateSuspendedStocks();

        Task<ICollection<StockBlockSale>> GetBlockSaleStocks();
        Task UpdateBlockSaleStocks();

        Task UpdateStocks(IEnumerable<string> stockCodes);

        void UpdateInvestagramsCookie(string value);

        DateTime GetLastTradingDate();

    }
}
