using System.Threading.Tasks;
using Lychee.Stocks.InvestagramsApi.Models.Calendar;

namespace Lychee.Stocks.InvestagramsApi.Interfaces
{
    public interface ICalendarApiRepository : IInvestagramsRestSharpBaseRepository
    {
        Task<CalendarOverview> GetCalendarOverview();
    }
}
