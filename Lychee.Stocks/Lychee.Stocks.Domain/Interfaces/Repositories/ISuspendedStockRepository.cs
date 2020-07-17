using System;
using System.Collections.Generic;
using Lychee.Stocks.InvestagramsApi.Models.Stocks;

namespace Lychee.Stocks.Domain.Interfaces.Repositories
{
    public interface ISuspendedStockRepository
    {
        void SaveSuspendedStocks(ICollection<SuspendedStock> suspendedStocks);

        DateTime? GetLastStockSuspensionDate();
    }
}
