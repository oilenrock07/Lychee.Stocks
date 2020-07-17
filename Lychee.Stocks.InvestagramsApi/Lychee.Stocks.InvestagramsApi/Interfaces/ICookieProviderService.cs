using System;
using System.Collections.Generic;

namespace Lychee.Stocks.InvestagramsApi.Interfaces
{
    public interface ICookieProviderService
    {
        Func<string> SetCookieFunc { get; set; }

        bool HasCookie { get; }
        void SetCookie(string cookie);

        Dictionary<string, string> GetCookieHeader();
    }
}
