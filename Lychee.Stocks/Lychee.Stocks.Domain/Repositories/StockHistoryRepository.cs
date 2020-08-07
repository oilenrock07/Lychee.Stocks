using System;
using System.Collections.Generic;
using System.Linq;
using Lychee.Infrastructure;
using Lychee.Infrastructure.Interfaces;
using Lychee.Stocks.Domain.Interfaces.Repositories;
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

        public virtual List<StockHistory> GetAllStocksWithSteepDown()
        {
            return ExecuteSqlQuery<StockHistory>("EXEC RetrieveStockSteepDown").ToList();
        }
    }

}
