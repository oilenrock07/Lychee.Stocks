using System;
using System.Collections.Generic;
using Lychee.Infrastructure.Interfaces;
using Lychee.Stocks.Entities;

namespace Lychee.Stocks.Domain.Interfaces.Repositories
{
    public interface IStockHistoryRepository : IRepository<StockHistory>
    {
        List<StockHistory> GetAllStocksByDate(DateTime date);
    }
}
