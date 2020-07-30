using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Lychee.Stocks.Domain.Interfaces.Services;
using Lychee.Stocks.InvestagramsApi.Interfaces;

namespace Lychee.Stocks.Controllers
{
    ////Takeaways and self note
    /// Only allow 2% of your total capital to be your loss

    ////Chart.js
    /// Clickable slice of pie chart: https://jsfiddle.net/u1szh96g/208/
    public class HomeController : Controller
    {
        private readonly IStockService _stockService;
        private readonly IInvestagramsApiCachedService _investagramsApiService;


        public HomeController(IStockService stockService, IInvestagramsApiCachedService investagramsApiService)
        {
            _stockService = stockService;
            _investagramsApiService = investagramsApiService;
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
            var news = await _investagramsApiService.GetNewsByStockId(-1);
            return PartialView("_News", news);
        }
    }
}