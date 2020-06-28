using System.Collections.Generic;

namespace Lychee.Stocks.InvestagramsApi.Interfaces
{
    public interface ICookieProviderService
    {
        void SetCookie(string cookie);

        Dictionary<string, string> GetCookieHeader();
    }
}
