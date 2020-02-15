using System;
using System.Collections.Generic;

namespace Lychee.Stocks.Domain.Interfaces.Repositories
{
    public interface IBlockSaleStockRepository
    {
        void SaveBlockSaleStocks(ICollection<Models.Investagrams.StockBlockSale> suspendedStocks);

        DateTime? GetLastBlockSaleStocksDate();
    }
}
