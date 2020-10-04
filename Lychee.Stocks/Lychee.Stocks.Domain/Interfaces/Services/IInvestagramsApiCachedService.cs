using System.Collections.Generic;
using System.Threading.Tasks;
using Lychee.Stocks.InvestagramsApi.Interfaces;
using Lychee.Stocks.InvestagramsApi.Models.Stocks;

namespace Lychee.Stocks.Domain.Interfaces.Services
{
    public interface IInvestagramsApiCachedService : IInvestagramsApiService
    {
        Task<List<ScreenerResponse>> GetAllLatestStocks();

        Task<List<ScreenerResponse>> GetMacdAboutToCrossFromBelowBullish();

        Task<List<ScreenerResponse>> GetMacdCrossingSignalFromBelowBullish();

        Task<List<ScreenerResponse>> GetOversoldStocks();

        Task<List<ScreenerResponse>> GetOversoldStocksLessThan20();

        Task<List<ScreenerResponse>> Get52WeekLow();

        Task<List<ScreenerResponse>> GreenVolume();
    }
}
