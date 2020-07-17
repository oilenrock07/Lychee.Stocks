using System;
using System.Collections.Generic;
using System.Linq;
using Lychee.Infrastructure.Interfaces;
using Lychee.Stocks.Domain.Interfaces.Repositories;
using Lychee.Stocks.Entities;
using Lychee.Stocks.InvestagramsApi.Models.Stocks;
using Omu.ValueInjecter;

namespace Lychee.Stocks.Domain.Repositories
{
    public class BlockSaleStockRepository : IBlockSaleStockRepository
    {
        private readonly IRepository<BlockSaleStock> _blockSaleStockRepository;

        public BlockSaleStockRepository(IRepository<BlockSaleStock> suspendedStockRepository)
        {
            _blockSaleStockRepository = suspendedStockRepository;
        }

        public void SaveBlockSaleStocks(ICollection<StockBlockSale> suspendedStocks)
        {
            foreach (var stocks in suspendedStocks)
            {
                var suspendedStock = Mapper.Map<BlockSaleStock>(stocks);
                _blockSaleStockRepository.Add(suspendedStock);
            }

            _blockSaleStockRepository.SaveChanges();
        }

        public DateTime? GetLastBlockSaleStocksDate()
        {
            var lastBlockSaleStock = _blockSaleStockRepository.GetAll().OrderByDescending(x => x.Id).FirstOrDefault();
            return lastBlockSaleStock?.Date;
        }
    }
}
