using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Lychee.Stocks.Domain.Interfaces.Services;
using Lychee.Stocks.InvestagramsApi.Interfaces;

namespace Lychee.Stocks.Controllers
{
    ////Takeaways and self note
    /// Only allow 2% of your total capital to be your loss

    public class HomeController : Controller
    {
        private readonly IStockService _stockService;
        private readonly IInvestagramsApiService _investagramsApiService;


        public HomeController(IStockService stockService, IInvestagramsApiService investagramsApiService)
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
        public PartialViewResult TrendingStocks()
        {
            var model = Task.Run(async () => await _investagramsApiService.GetTrendingStocks()).Result;
            return PartialView(model);
        }

        public PartialViewResult Dividends()
        {
            var model = Task.Run(async () => await _stockService.GetStocksGivingDividends()).Result;
            return PartialView(model);
        }
    }
}