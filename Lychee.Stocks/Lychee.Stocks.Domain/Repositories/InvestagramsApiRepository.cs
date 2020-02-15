using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Lychee.CommonHelper.Extensions;
using Lychee.Domain.Interfaces;
using Lychee.HttpClientService;
using Lychee.Stocks.Domain.Constants;
using Lychee.Stocks.Domain.Exception;
using Lychee.Stocks.Domain.Interfaces.Repositories;
using Lychee.Stocks.Domain.Models.Investagrams;

namespace Lychee.Stocks.Domain.Repositories
{
    public class InvestagramsApiRepository : IInvestagramsApiRepository, IStockDataRepository
    {
        private readonly string _stockApiPath = "/InvestaApi/Stock";

        private readonly ISettingRepository _settingRepository;
        private readonly IHttpClientService _httpClientService;

        private readonly string _cookieInvalidMessage = "Please Login to Continue.";
        private readonly string _cookieErrorMessage = "Cookie is expired";

        public InvestagramsApiRepository(ISettingRepository settingRepository, IHttpClientService httpClientService)
        {
            _settingRepository = settingRepository;
            _httpClientService = httpClientService;
        }

        public async Task<LatestStockMarketActivityVm> GetLatestStockMarketActivity()
        {
            var headers = GetInvestagramsHeaderRequest();
            var result = await _httpClientService.SendRequest<LatestStockMarketActivityVm>($"{_stockApiPath}/getLatestStockMarketActivityVM?exchangeType=1", HttpMethod.Post, headers).ConfigureAwait(false);

            if (!string.IsNullOrEmpty(result.Message) && result.Message.EqualsTo(_cookieInvalidMessage))
                throw new InvestagramsException(_cookieErrorMessage);

            return result;
        }

        public async Task<ViewStock> ViewStock(string stockCode)
        {
            var headers = GetInvestagramsHeaderRequest();
            var result = await _httpClientService.SendRequest<ViewStock>($"{_stockApiPath}/viewStock?stockCode=PSE:{stockCode}", HttpMethod.Post, headers).ConfigureAwait(false);
            return result;
        }

        protected virtual Dictionary<string, string> GetInvestagramsHeaderRequest()
        {
            var cookie = _settingRepository.GetSettingValue<string>(SettingNames.InvestagramsCookieName);
            var headers = new Dictionary<string, string>
            {
                {"cookie", cookie},
                {"origin", UrlConstants.InvestagramsBaseUrl}
            };

            return headers;
        }
    }


}
