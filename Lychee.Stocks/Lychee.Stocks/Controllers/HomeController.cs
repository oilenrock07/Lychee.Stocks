using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Lychee.Stocks.Common.Interfaces;
using Lychee.Stocks.Domain.Interfaces.Services;
using Lychee.Stocks.Helpers;
using Lychee.Stocks.InvestagramsApi.Interfaces;
using Lychee.Stocks.InvestagramsApi.Models.Stocks;
using Lychee.Stocks.Models.Stocks;

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
            var latestStockData = _stockService.GetLatestStockHistory();


            var viewModel = model.Select(x =>
            {
                var stock = latestStockData.ContainsKey(x.Stock.StockCode) ? latestStockData[x.Stock.StockCode] : null;

                return new TrendingStockViewModel
                {
                    StockCode = x.Stock.StockCode,
                    Last = stock?.Last ?? 0,
                    Open = stock?.Open ?? 0,
                    Trades = stock?.Trades ?? 0
                };
            }).ToList();


            return PartialView(viewModel);
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
            stocks = GetStocksWithAverageTradesAbove100(stocks);
            return PartialView(stocks);
        }

        public async Task<PartialViewResult> AboutToCrossMacd()
        {
            var stocks = await _investagramsApiService.GetMacdAboutToCrossFromBelowBullish();
            stocks = GetStocksWithAverageTradesAbove100(stocks);
            ViewBag.Title = "MACD About to Cross";
            return PartialView("_MACD", stocks);
        }

        public async Task<PartialViewResult> CrossingMacd()
        {
            var stocks = await _investagramsApiService.GetMacdCrossingSignalFromBelowBullish();
            stocks = GetStocksWithAverageTradesAbove100(stocks);
            ViewBag.Title = "MACD Crossing";
            return PartialView("_MACD", stocks);
        }

        public async Task<PartialViewResult> FiftyTwoWeekLow()
        {
            var stocks = await _investagramsApiService.Get52WeekLow();
            stocks = GetStocksWithAverageTradesAbove100(stocks);
            return PartialView(stocks);
        }

        public PartialViewResult SteepDown()
        {
            var stocks = _stockService.GetStockWithSteepDown();
            stocks = GetStocksWithAverageTradesAbove100(stocks);
            return PartialView(stocks);
        }

        public PartialViewResult MorningStarDoji()
        {
            var stocks = _stockService.GetMorningStarDoji();
            stocks = GetStocksWithAverageTradesAbove100(stocks);
            ViewBag.Header = "Morning Star Doji";
            return PartialView("_BasicChartHistoryDisplay", stocks);
        }

        public PartialViewResult Hammers()
        {
            var stocks = _stockService.GetHammers();
            stocks = GetStocksWithAverageTradesAbove100(stocks);
            ViewBag.Header = "Hammers";
            return PartialView("_BasicChartHistoryDisplay", stocks);
        }

        public async Task<PartialViewResult> GreenVolume()
        {
            var stocks = await _investagramsApiService.GreenVolume();
            stocks = GetStocksWithAverageTradesAbove100(stocks);
            return PartialView("GreenVolume", stocks);
        }

        public JsonResult Top10Trades()
        {
            var stocks =  _stockService.GetTop10HighestTrades();
            return Json(stocks);
        }

        private List<T> GetStocksWithAverageTradesAbove100<T>(List<T> list) where T: IStock
        {
            var stocksWithAvgTradesAbove100 = _stockService.GetStockTradeAverages(2, 100);
            return list.Where(x => stocksWithAvgTradesAbove100.Select(s => s.StockCode).Contains(x.StockCode)).ToList();
        }
    }
}