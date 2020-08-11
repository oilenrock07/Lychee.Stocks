using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Lychee.Stocks.Domain.Interfaces.Services;
using Lychee.Stocks.Helpers;
using Lychee.Stocks.InvestagramsApi.Interfaces;
using Lychee.Stocks.InvestagramsApi.Models.Stocks;

namespace Lychee.Stocks.Controllers
{
    ////Takeaways and self note
    /// Only allow 2% of your total capital to be your loss
    
    /// StockPrice == High && Buyers > Sellers then Price will go down
    /// StockPrice == Low &&  Buyers > Sellers then Price will go up

    ////Chart.js
    /// Clickable slice of pie chart: https://jsfiddle.net/u1szh96g/208/
    public class HomeController : Controller
    {
        private readonly IStockService _stockService;
        private readonly IInvestagramsApiCachedService _investagramsApiService;
        private readonly IWatchListService _watchListService;

        public HomeController(IStockService stockService, IInvestagramsApiCachedService investagramsApiService, IWatchListService watchListService)
        {
            _stockService = stockService;
            _investagramsApiService = investagramsApiService;
            _watchListService = watchListService;
        }

        public async Task<ActionResult> Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //Cannot use Task<PartialViewResult> on child action
        //https://justsimplycode.com/2016/09/04/child-action-in-mvc-5-does-not-support-async/
        public async Task<PartialViewResult> TrendingStocks()
        {
            var model = await _investagramsApiService.GetTrendingStocks();
            return PartialView(model);
        }

        public PartialViewResult Dividends()
        {
            var model = Task.Run(async () => await _stockService.GetStocksGivingDividends()).Result;
            return PartialView(model);
        }

        public async Task<PartialViewResult> News()
        {
            var news = new List<News>();
            var stockNews = await _investagramsApiService.GetNewsByStockId(-1);
            var disclosureNews = await _investagramsApiService.GetDisclosureNews();

            news.AddRange(stockNews);
            news.AddRange(disclosureNews);
            var watchList = _watchListService.GetAllWatchList();

            var viewModels = NewsHelper.GetNewsViewModels(news, watchList);

            return PartialView("_News", viewModels);
        }

        public async Task<PartialViewResult> Oversold()
        {
            var stocks = await _investagramsApiService.GetOversoldStocksLessThan20();
            return PartialView(stocks);
        }

        public async Task<PartialViewResult> AboutToCrossMacd()
        {
            var stocks = await _investagramsApiService.GetMacdAboutToCrossFromBelowBullish();
            return PartialView("_MACD", stocks);
        }

        public async Task<PartialViewResult> CrossingMacd()
        {
            var stocks = await _investagramsApiService.GetMacdCrossingSignalFromBelowBullish();
            return PartialView("_MACD", stocks);
        }

        public async Task<PartialViewResult> FiftyTwoWeekLow()
        {
            var stocks = await _investagramsApiService.Get52WeekLow();
            return PartialView(stocks);
        }

        public PartialViewResult SteepDown()
        {
            var stocks = _stockService.GetStockWithSteepDown();
            return PartialView(stocks);
        }
    }
}