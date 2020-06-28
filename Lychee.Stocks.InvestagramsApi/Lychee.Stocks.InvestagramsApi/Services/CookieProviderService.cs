using System;
using System.Collections.Generic;
using System.Linq;
using Lychee.Stocks.InvestagramsApi.Interfaces;

namespace Lychee.Stocks.InvestagramsApi.Services
{
    public class CookieProviderService : ICookieProviderService
    {
        private string _cookie = string.Empty;

        public CookieProviderService()
        {
            
        }

        public CookieProviderService(string cookie)
        {
            _cookie = cookie;
        }

        public virtual void SetCookie(string cookie)
        {
            _cookie = cookie;
        }

        public virtual Dictionary<string, string> GetCookieHeader()
        {
            if (string.IsNullOrEmpty(_cookie))
                throw new Exception("Cookie is not set");

            var array = _cookie.Split(';');
            return array.Select(data => data.Split('=')).ToDictionary(d => d[0].TrimStart(), d => d[1]);
        }
    }
}
