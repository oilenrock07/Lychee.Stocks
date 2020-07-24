using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lychee.Stocks.InvestagramsApi.Interfaces;
using Lychee.Stocks.InvestagramsApi.Models.Stocks;

namespace Lychee.Stocks.Domain.Services
{
    public class StockNewsService
    {
        private readonly IInvestagramsApiService _investagramsApiService;

        public StockNewsService(IInvestagramsApiService investagramsApiService)
        {
            _investagramsApiService = investagramsApiService;
        }

        public async Task<List<News>> GetAllNews()
        {
            return await _investagramsApiService.GetDisclosureNews();
        }
    }
}
