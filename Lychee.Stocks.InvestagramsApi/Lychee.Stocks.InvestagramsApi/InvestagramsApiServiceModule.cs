using Lychee.Stocks.InvestagramsApi.Interfaces;
using Lychee.Stocks.InvestagramsApi.Repositories;
using Lychee.Stocks.InvestagramsApi.Services;
using SimpleInjector;

namespace Lychee.Stocks.InvestagramsApi
{
    public static class InvestagramsApiServiceModule
    {
        public static void RegisterInvestagramsApi(this Container container)
        {
            container.RegisterSingleton<ICalendarApiRepository, CalendarApiRepository>();
            container.RegisterSingleton<ISocialApiRepository, SocialApiRepository>();
            container.RegisterSingleton<IStockApiRepository, StockApiRepository>();

            container.RegisterSingleton<IInvestagramsApiService, InvestagramsApiService>();
            container.RegisterSingleton<ICookieProviderService, CookieProviderService>();
        }
    }
}
