using System;
using System.Collections.Generic;
using System.Linq;
using Lychee.Infrastructure;
using Lychee.Infrastructure.Interfaces;
using Lychee.Stocks.Domain.Interfaces.Repositories;
using Lychee.Stocks.Domain.Models;
using Lychee.Stocks.Entities;

namespace Lychee.Stocks.Domain.Repositories
{
    public class StockHistoryRepository : Repository<StockHistory>, IStockHistoryRepository
    {
        public StockHistoryRepository(IDatabaseFactory databaseFactory) : base(databaseFactory.GetContext())
        {
        }

        public virtual List<StockHistory> GetAllStocksByDate(DateTime date)
        {
            date = date.Date;
            return Find(x => x.Date == date).ToList();
        }

        public virtual List<StockTradeAverage> GetAverageStocks(int averageDays, int averageTrades)
        {
            return ExecuteSqlQuery<StockTradeAverage>($"EXEC RetrieveAverageStocks {averageDays}, {averageTrades}").ToList();
        }

        public virtual List<StockHistory> GetTopXTradeHistory(DateTime date, int topX)
        {
            return ExecuteSqlQuery<StockHistory>($"EXEC RetrieveTopXStockHistory '{date:yyyy-MM-dd}', {topX}").ToList();
        }

        public virtual List<StockHistory> GetAllDojis()
        {
            return ExecuteSqlQuery<StockHistory>("EXEC RetrieveAllDojis").ToList();
        }

        public virtual List<StockHistory> GetAllHammers()
        {
            return ExecuteSqlQuery<StockHistory>("EXEC RetrieveAllHammers").ToList();
        }
    }

}
