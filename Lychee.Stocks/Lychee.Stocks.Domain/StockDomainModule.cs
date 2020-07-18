using Lychee.Stocks.Domain.Interfaces.Repositories;
using Lychee.Stocks.Domain.Interfaces.Services;
using Lychee.Stocks.Domain.Repositories;
using Lychee.Stocks.Domain.Services;
using SimpleInjector;

namespace Lychee.Stocks.Domain
{
    public static class StockDomainModule
    {
        public static void RegisterStocksDomain(this Container container)
        {
            container.Register<ISuspendedStockRepository, SuspendedStockRepository>(Lifestyle.Scoped);
            container.Register<IBlockSaleStockRepository, BlockSaleStockRepository>(Lifestyle.Scoped);
            container.Register<IStockHistoryRepository, StockHistoryRepository>(Lifestyle.Scoped);

            container.Register<IStockScoreService, StockScoreService>(Lifestyle.Scoped);
        }
    }
}
