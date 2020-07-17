using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Reflection;
using System.Web.Mvc;
using Lychee.Caching;
using Lychee.Caching.Interfaces;
using Lychee.Domain;
using Lychee.Domain.Interfaces;
using Lychee.HttpClientService;
using Lychee.Infrastructure;
using Lychee.Infrastructure.Interfaces;
using Lychee.Scrapper.Domain.Interfaces;
using Lychee.Scrapper.Domain.Services;
using Lychee.Scrapper.Repository.Interfaces;
using Lychee.Scrapper.Repository.Repositories;
using Lychee.Stocks.Domain.Constants;
using Lychee.Stocks.Domain.Interfaces.Repositories;
using Lychee.Stocks.Domain.Interfaces.Services;
using Lychee.Stocks.Domain.Repositories;
using Lychee.Stocks.Domain.Services;
using Lychee.Stocks.Entities;
using Lychee.Stocks.InvestagramsApi;
using Lychee.Stocks.InvestagramsApi.Interfaces;
using Serilog;
using Serilog.Core;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Lifestyles;

namespace Lychee.Stocks
{
    public class WebModuleConfig
    {
        public static void RegisterModules()
        {
            var loggingPath = Path.Combine(ConfigurationManager.AppSettings["LoggingPath"], "Stocks", "Log.txt");

            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
            container.Register<DbContext>(() => new StockContext(), Lifestyle.Scoped);
            container.Register<Logger>(() => new LoggerConfiguration().WriteTo.File(loggingPath).CreateLogger(), Lifestyle.Singleton);
            container.Register(typeof(IRepository<>), typeof(Repository<>), Lifestyle.Scoped);
            container.Register<IDatabaseFactory, DatabaseFactory>(Lifestyle.Scoped);


            //Repositories
            container.RegisterSingleton<IScrappedSettingRepository, ScrappedSettingRepository>();
            container.RegisterSingleton<IHeaderRequestRepository, HeaderRequestRepository>();
            container.RegisterSingleton<IColumnDefinitionRepository, ColumnDefinitionRepository>();
            container.RegisterSingleton<IScrappedDataRepository, ScrappedDataRepository>();
            container.Register<ISuspendedStockRepository, SuspendedStockRepository>(Lifestyle.Scoped);
            container.Register<IBlockSaleStockRepository, BlockSaleStockRepository>(Lifestyle.Scoped);

            //Services
            container.RegisterSingleton<ILoggingService, LoggingService>();
            container.RegisterSingleton<IWebQueryService, WebQueryService>();
            container.Register<IStockService, StockService>(Lifestyle.Scoped);
            container.Register<IPredictionService, PredictionService>(Lifestyle.Scoped);
            container.Register<IResultCollectionService, ResultCollectionService>(Lifestyle.Scoped);
            container.RegisterSingleton<ICachingFactory, CachingFactory>();

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            //Lychee Packages
            container.RegisterLycheeHttpClientService(UrlConstants.InvestagramsWebApiBaseUrl);
            container.RegisterLycheeDomainServiceModule();
            container.RegisterInvestagramsApi();

            container.Verify();
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

            SetCookieFunc(container);
        }


        private static void SetCookieFunc(Container container)
        {
            using (var scope = AsyncScopedLifestyle.BeginScope(container))
            {
                if (scope.Container != null)
                {
                    var cookieProviderService = scope.Container.GetInstance<ICookieProviderService>();
                    var settingService = scope.Container.GetInstance<ISettingService>();
                    cookieProviderService.SetCookieFunc = () => settingService.GetSettingValue<string>(SettingNames.InvestagramsCookieName);
                }
            }
        }
    }
}