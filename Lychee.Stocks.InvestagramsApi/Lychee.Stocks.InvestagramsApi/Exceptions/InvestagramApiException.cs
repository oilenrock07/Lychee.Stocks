using System;

namespace Lychee.Stocks.InvestagramsApi.Exceptions
{
    public class InvestagramApiException : Exception
    {
        public InvestagramApiException()
        {
            
        }

        public InvestagramApiException(string message) : base(message)
        {
            
        }
    }
}
