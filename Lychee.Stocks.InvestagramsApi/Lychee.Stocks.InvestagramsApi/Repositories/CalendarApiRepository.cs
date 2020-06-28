using System.Threading.Tasks;
using Lychee.Stocks.InvestagramsApi.Interfaces;
using Lychee.Stocks.InvestagramsApi.Models.Calendar;
using RestSharp;

namespace Lychee.Stocks.InvestagramsApi.Repositories
{
    public class CalendarApiRepository : InvestagramsRestsharpBaseRepository, ICalendarApiRepository
    {
        private readonly string _stockApiPath = "/InvestaApi/Calendar";

        public CalendarApiRepository(ICookieProviderService cookieProviderService) : base(cookieProviderService)
        {
        }

        public async Task<CalendarOverview> GetCalendarOverview()
        {
            var result = await PostToApi<CalendarOverview>($"{_stockApiPath}/GetCalendarOverview?region=XX", Method.POST);
            return result.Data;
        }


    }
}
