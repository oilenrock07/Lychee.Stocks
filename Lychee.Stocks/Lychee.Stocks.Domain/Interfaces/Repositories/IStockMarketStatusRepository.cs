using System;

namespace Lychee.Stocks.Domain.Interfaces.Repositories
{
    public interface IStockMarketStatusRepository
    {
        DateTime GetLastTradingDate();
    }
}
