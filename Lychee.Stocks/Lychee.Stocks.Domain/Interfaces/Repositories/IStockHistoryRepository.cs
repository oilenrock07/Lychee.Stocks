﻿using System;
using System.Collections.Generic;
using Lychee.Infrastructure.Interfaces;
using Lychee.Stocks.Domain.Models;
using Lychee.Stocks.Entities;

namespace Lychee.Stocks.Domain.Interfaces.Repositories
{
    public interface IStockHistoryRepository : IRepository<StockHistory>
    {
        List<StockHistory> GetAllStocksByDate(DateTime date);

        List<StockTradeAverage> GetAverageStocks(int averageDays, int averageTrades);

        List<StockHistory> GetTopXTradeHistory(DateTime date, int topX);

        List<StockHistory> GetAllDojis();

        List<StockHistory> GetAllHammers();

        List<StockHistory> RetrieveStockLastHistory();

        List<StockHistory> GetTop10HighesTrades(DateTime date);

        List<StockHistory> GetTop10HighesVolumes(DateTime date);

        List<StockHistory> GetTop10HighesValue(DateTime date);
    }
}
