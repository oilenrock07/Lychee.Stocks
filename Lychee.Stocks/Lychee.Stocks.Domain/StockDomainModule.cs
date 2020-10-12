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
            container.Register<IStockHistoryRepository, StockHistoryRepository>(Lifestyle.Scoped);
            container.Register<IWatchListRepository, WatchListRepository>(Lifestyle.Scoped);
            container.Register<IEventRepository, EventRepository>(Lifestyle.Scoped);
            container.Register<IStockMarketStatusRepository, StockMarketStatusRepository>(Lifestyle.Scoped);
            

            container.Register<IStockScoreService, StockScoreService>(Lifestyle.Scoped);
            container.Register<IWatchListService, WatchListService>(Lifestyle.Scoped);
            container.Register<IInvestagramsApiCachedService, InvestagramsApiCachedService>(Lifestyle.Scoped);
            container.Register<ICandleStickAnalyzerService, CandleStickAnalyzerService>(Lifestyle.Scoped);
        }
    }
}
