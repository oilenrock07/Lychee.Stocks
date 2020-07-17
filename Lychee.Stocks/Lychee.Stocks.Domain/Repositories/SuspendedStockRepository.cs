using System;
using System.Collections.Generic;
using System.Linq;
using Lychee.Infrastructure.Interfaces;
using Lychee.Stocks.Domain.Interfaces.Repositories;
using Omu.ValueInjecter;
using SuspendedStock = Lychee.Stocks.Entities.SuspendedStock;

namespace Lychee.Stocks.Domain.Repositories
{
    public class SuspendedStockRepository : ISuspendedStockRepository
    {
        private readonly IRepository<SuspendedStock> _suspendedStockRepository;

        public SuspendedStockRepository(IRepository<SuspendedStock> suspendedStockRepository)
        {
            _suspendedStockRepository = suspendedStockRepository;
        }

        public void SaveSuspendedStocks(ICollection<Lychee.Stocks.InvestagramsApi.Models.Stocks.SuspendedStock> suspendedStocks)
        {
            foreach (var stocks in suspendedStocks)
            {
                var suspendedStock = Mapper.Map<SuspendedStock>(stocks);
                _suspendedStockRepository.Add(suspendedStock);
            }

            _suspendedStockRepository.SaveChanges();
        }

        public DateTime? GetLastStockSuspensionDate()
        {
            var lastSuspendedStock = _suspendedStockRepository.GetAll().OrderByDescending(x => x.Id).FirstOrDefault();
            return lastSuspendedStock?.Date;
        }
    }
}
