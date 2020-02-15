using System;
using System.Collections.Generic;

namespace Lychee.Stocks.Domain.Interfaces.Repositories
{
    public interface ISuspendedStockRepository
    {
        void SaveSuspendedStocks(ICollection<Models.Investagrams.SuspendedStock> suspendedStocks);

        DateTime? GetLastStockSuspensionDate();
    }
}
