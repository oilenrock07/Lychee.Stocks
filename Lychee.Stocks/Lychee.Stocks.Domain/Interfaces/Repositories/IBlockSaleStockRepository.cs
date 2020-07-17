using System;
using System.Collections.Generic;
using Lychee.Stocks.InvestagramsApi.Models.Stocks;

namespace Lychee.Stocks.Domain.Interfaces.Repositories
{
    public interface IBlockSaleStockRepository
    {
        void SaveBlockSaleStocks(ICollection<StockBlockSale> suspendedStocks);

        DateTime? GetLastBlockSaleStocksDate();
    }
}
