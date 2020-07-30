using System.Threading.Tasks;
using System.Web.Mvc;
using Lychee.Stocks.Domain.Interfaces.Services;

namespace Lychee.Stocks.Controllers
{
    public class NewsController : Controller
    {
        private readonly IInvestagramsApiCachedService _investagramsApi;

        public NewsController(IInvestagramsApiCachedService investagramsApi)
        {
            _investagramsApi = investagramsApi;
        }

        public async Task<ActionResult> Disclosure(int lastPage = -1)
        {
            var news = await _investagramsApi.GetDisclosureNews(lastPage);
            return View(news);
        }

        public async Task<ActionResult> FinancialReport(int lastPage = -1)
        {
            var news = await _investagramsApi.GetFinancialReportNews(lastPage);
            return View(news);
        }

        public async Task<ActionResult> Stock(int lastPage = -1)
        {
            var news = await _investagramsApi.GetNewsByStockId(lastPage);
            return View(news);
        }

        public async Task<ActionResult> Business(int lastPage = -1)
        {
            var news = await _investagramsApi.GetBusinessNews(lastPage);
            return View(news);
        }
    }
}