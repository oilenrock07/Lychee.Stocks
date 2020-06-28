using System;
using System.Collections.Generic;

namespace Lychee.Stocks.InvestagramsApi.Interfaces
{
    public interface IInvestagramsRestSharpBaseRepository
    {
        Func<Dictionary<string, string>> AddCookies { get; set; }

        Dictionary<string, string> ParseCookie(string cookie);
    }
}
