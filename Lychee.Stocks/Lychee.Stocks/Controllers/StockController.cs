using System.Threading.Tasks;
using System.Web.Mvc;
using Lychee.Domain.Interfaces;
using Lychee.Stocks.Domain.Constants;
using Lychee.Stocks.Domain.Interfaces.Services;
using Lychee.Stocks.Models;
using Lychee.Stocks.Models.Stocks;

namespace Lychee.Stocks.Controllers
{
    public class StockController : Controller
    {
        private readonly IStockService _stockService;
        private readonly ISettingService _settingService;

        public StockController(IStockService stockService, ISettingService settingService)
        {
            _stockService = stockService;
            _settingService = settingService;
        }

        public async Task<ActionResult> FetchRealTimeData()
        {
            //make this an ajax request and return json. Maybe publish the changes as toast.
            //at the moment this will just display the yellow page of death
            //when you have time, convert this to use bootstrap 4 and use toast https://getbootstrap.com/docs/4.3/components/toasts/
            await _stockService.SaveLatestStockUpdate();

            TempData["FetchRealTimeData"] = "Success";
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }


        public async Task<ActionResult> ShouldIBuyStock(string stockCode = "")
        {
            var viewModel = new ShouldIBuyStockViewModel { StockCode = stockCode};
            if (string.IsNullOrEmpty(stockCode))
                return View(viewModel);

            var score = await _stockService.GetStockTotalScore(stockCode);

            var passingScore = _settingService.GetSettingValue<decimal>(SettingNames.Score_ShouldIBuyStockPassingScore);
            viewModel.ShouldIBuyStock = score.TotalScore >= passingScore || score.HasSignificantUptrendReason ? "Yes" : "No";
            viewModel.UpTrendReasons = score.UpTrendReasons;
            viewModel.DownTrendReasons = score.DownTrendReasons;

            return View(viewModel);
        }

        public async Task<ActionResult> GetDataUpdates()
        {
            await _stockService.UpdateSuspendedStocks();
            await _stockService.UpdateBlockSaleStocks();

            TempData["FetchRealTimeData"] = "Success";
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public ActionResult LastDataUpdates()
        {
            var date = _stockService.GetLastDataUpdates();
            var viewModel = new LastDataUpdateViewModel
            {
                LastStockHistoryUpdate = date
            };
            return PartialView(viewModel);
        }

        public ActionResult StockTrendReport(int days, int losingWinningStreak, string trend)
        {
            var result = _stockService.GetStockTrendReport(days, losingWinningStreak, trend);
            return PartialView(result);
        }
    }
}