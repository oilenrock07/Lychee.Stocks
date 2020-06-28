using System;
using System.Net;
using System.Threading.Tasks;
using Lychee.Models;
using Lychee.Models.Interfaces;
using Lychee.Stocks.InvestagramsApi.Constants;
using Lychee.Stocks.InvestagramsApi.Interfaces;
using RestSharp;

namespace Lychee.Stocks.InvestagramsApi.Repositories
{
    public abstract class InvestagramsRestsharpBaseRepository
    {
        private readonly string _investagramsBaseUrl = "https://webapi.investagrams.com";
        private readonly ICookieProviderService _cookieProviderService;

        public InvestagramsRestsharpBaseRepository(ICookieProviderService cookieProviderService)
        {
            _cookieProviderService = cookieProviderService;
        }

        public virtual async Task<IResultData<T>> PostToApi<T>(string url, Method method)
        {
            return await PostToApi<T>(null, url, method);
        }

        public virtual async Task<IResultData<T>> PostToApi<T>(object data, string url, Method method)
        {
            var request = GetRestRequest(url, method);
            if (data != null)
            {
                var serializedData = Utf8Json.JsonSerializer.Serialize(data);

                request.AddHeader("content-length", serializedData.Length.ToString());
                request.AddParameter("application/json", serializedData, "application/json", ParameterType.RequestBody);
            }
            else
            {
                request.AddHeader("content-length", "0");
            }

            var response = await Execute(request);
            if (response == null || !response.IsSuccessful)
            {
                return new ResultData<T>(default(T),
                    (int)(response?.StatusCode ?? HttpStatusCode.ServiceUnavailable), 
                    response?.ErrorMessage ?? "",
                    response?.ErrorException ?? new Exception("Request Failed"),
                    response?.Content);
            }


            var checkout = Utf8Json.JsonSerializer.Deserialize<T>(response.Content);
            return new ResultData<T>(checkout, (int)response.StatusCode,
                response.ErrorMessage, response.ErrorException, response.Content);
        }
        
        protected virtual async Task<IRestResponse> Execute(RestRequest request)
        {
            var client = GetRestClient();
            var response = await client.ExecuteAsync(request);
            return response;
        }

        protected virtual RestClient GetRestClient()
        {
            return new RestClient();
        }

        protected virtual RestRequest GetRestRequest(string endpoint, Method method)
        {
            var url = $"{_investagramsBaseUrl}{endpoint}";
            var req = new RestRequest(url, method);

            var cookies = _cookieProviderService.GetCookieHeader();
            foreach (var cookie in cookies)
            {
                req.AddParameter(cookie.Key, cookie.Value, ParameterType.Cookie);
            }

            req.AddHeader("origin", UrlConstants.InvestagramsBaseUrl);
            return req;
        }
    }
}
