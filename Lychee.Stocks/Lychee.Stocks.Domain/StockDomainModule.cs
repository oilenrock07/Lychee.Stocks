using Lychee.Stocks.Domain.Interfaces.Repositories;
using Lychee.Stocks.Domain.Interfaces.Services;
using Lychee.Stocks.Domain.Repositories;
using Lychee.Stocks.Domain.Services;
using Lychee.Stocks.InvestagramsApi.Interfaces;
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
            container.RegisterSingleton<IStockMarketStatusRepository, StockMarketStatusRepository>();


            container.Register<IStockScoreService, StockScoreService>(Lifestyle.Scoped);
            container.Register<IInvestagramsApiService, InvestagramsApiCachedService>(Lifestyle.Scoped);
        }
    }
}
