using System.Collections.Generic;
using System.Threading.Tasks;
using Lychee.Stocks.InvestagramsApi.Interfaces;
using Lychee.Stocks.InvestagramsApi.Models.Social;
using RestSharp;

namespace Lychee.Stocks.InvestagramsApi.Repositories
{
    public class SocialApiRepository : InvestagramsRestsharpBaseRepository, ISocialApiRepository
    {
        private readonly string _stockApiPath = "/InvestaApi/Social";

        public async Task<List<TrendingStock>> GetTrendingStocks()
        {
            var result = await PostToApi<List<TrendingStock>>($"{_stockApiPath}/GetSocialPostTrendingStocksByExchange?exchangeType=1", Method.POST);
            return result.Data;
        }
    }
}
