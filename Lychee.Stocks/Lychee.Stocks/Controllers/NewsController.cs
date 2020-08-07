using System.Threading.Tasks;
using System.Web.Mvc;
using Lychee.Stocks.Domain.Interfaces.Services;
using Lychee.Stocks.Helpers;

namespace Lychee.Stocks.Controllers
{
    public class NewsController : Controller
    {
        private readonly IInvestagramsApiCachedService _investagramsApi;
        private readonly IWatchListService _watchListService;

        public NewsController(IInvestagramsApiCachedService investagramsApi, IWatchListService watchListService)
        {
            _investagramsApi = investagramsApi;
            _watchListService = watchListService;
        }

        public async Task<ActionResult> Disclosure(int lastPage = -1)
        {
            var news = await _investagramsApi.GetDisclosureNews(lastPage);
            var watchList = _watchListService.GetAllWatchList();

            var viewModels = NewsHelper.GetNewsViewModels(news, watchList);
            return View(viewModels);
        }

        public async Task<ActionResult> FinancialReport(int lastPage = -1)
        {
            var news = await _investagramsApi.GetFinancialReportNews(lastPage);
            var watchList = _watchListService.GetAllWatchList();

            var viewModels = NewsHelper.GetNewsViewModels(news, watchList);
            return View(viewModels);
        }

        public async Task<ActionResult> Stock(int lastPage = -1)
        {
            var news = await _investagramsApi.GetNewsByStockId(lastPage);
            var watchList = _watchListService.GetAllWatchList();

            var viewModels = NewsHelper.GetNewsViewModels(news, watchList);
            return View(viewModels);
        }

        public async Task<ActionResult> Business(int lastPage = -1)
        {
            var news = await _investagramsApi.GetBusinessNews(lastPage);
            var watchList = _watchListService.GetAllWatchList();

            var viewModels = NewsHelper.GetNewsViewModels(news, watchList);
            return View(viewModels);
        }
    }
}