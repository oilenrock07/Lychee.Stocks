using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Lychee.Domain.Interfaces;
using Lychee.Infrastructure.Interfaces;
using Lychee.Stocks.Domain.Interfaces.Services;
using Lychee.Stocks.Domain.Models;
using Lychee.Stocks.Entities;
using Lychee.Stocks.Models;
using Omu.ValueInjecter;

namespace Lychee.Stocks.Controllers
{
    public class StockController : Controller
    {
        private readonly IStockService _stockService;
        private readonly ISettingService _settingService;
        private readonly IRepository<Stock> _stockRepository;

        public StockController(IStockService stockService, ISettingService settingService, IRepository<Stock> stockRepository)
        {
            _stockService = stockService;
            _settingService = settingService;
            _stockRepository = stockRepository;
        }

        public ActionResult List()
        {
            var models = _stockRepository.GetAll().ToList().OrderBy(x => x.StockCode);
            return View(models);
        }

        public JsonResult UpdateStock(Stock model)
        {
            var stock = _stockRepository.GetById(model.StockId);
            _stockRepository.Attach(stock);
            stock.InjectFrom(model);
            _stockRepository.SaveChanges();

            return Json(new {success = true}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateStock()
        {
            return View(new Stock());
        }

        [HttpPost]
        public ActionResult CreateStock(Stock model)
        {
            _stockRepository.Add(model);
            _stockRepository.SaveChanges();
            TempData["Success"] = $"{model.StockCode} has been successfully created";

            return RedirectToAction("List");
        }

        public ActionResult DeleteStock(int stockId)
        {
            var stock = _stockRepository.GetById(stockId);
            _stockRepository.Delete(stock);
            _stockRepository.SaveChanges();

            TempData["Success"] = $"{stock.StockCode} has been successfully deleted";
            return RedirectToAction("List");
        }

        public ActionResult FetchRealTimeData()
        {
            //make this an ajax request and return json. Maybe publish the changes as toast.
            //at the moment this will just display the yellow page of death
            //when you have time, convert this to use bootstrap 4 and use toast https://getbootstrap.com/docs/4.3/components/toasts/
            _stockService.SaveLatestStockUpdate();

            TempData["FetchRealTimeData"] = "Success";
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }


        public async Task<ActionResult> StockAnalysis(string stockCode = "")
        {
            var viewModel = new StockAnalysisModel { StockCode = stockCode};
            if (string.IsNullOrEmpty(stockCode))
                return View(viewModel);

            viewModel = await _stockService.AnalyzeStock(stockCode);
            return View(viewModel);
        }

        public async Task<ActionResult> StockAnalysisTrending()
        {
            var viewModel = await _stockService.AnalyzeTrendingStock();
            return View(viewModel);
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

        public async Task<ActionResult> UpdateAllStocks()
        {
            try
            {
                await _stockService.UpdateAllStock();
            }
            catch (Exception ex)
            {

            }
            return View();
        }
    }
}