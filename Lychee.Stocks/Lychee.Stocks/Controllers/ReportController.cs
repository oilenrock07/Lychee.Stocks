using System;
using System.Linq;
using System.Web.Mvc;
using Lychee.Stocks.Domain.Interfaces.Services;
using Lychee.Stocks.Models.Reports;

namespace Lychee.Stocks.Controllers
{
    public class ReportController : Controller
    {
        private readonly IStockService _stockService;
        private readonly IPredictionService _predictionService;

        public ReportController(IStockService stockService, IPredictionService predictionService)
        {
            _stockService = stockService;
            _predictionService = predictionService;
        }

        public ActionResult DownTrends()
        {
            var fiveOverFive = _stockService.GetStockTrendReport(5, 5).ToList();
            var tenOverEight = _stockService.GetStockTrendReport(10, 8).ToList();
            var twentyOveryFifteen = _stockService.GetStockTrendReport(20, 15).ToList();
            var thirtyOverTwenty = _stockService.GetStockTrendReport(30, 20).ToList();

            var viewModel = new DownTrendReportViewModel
            {
                FiveOverFive = fiveOverFive,
                TenOverEight = tenOverEight,
                ThirtyOverTwenty = thirtyOverTwenty,
                TwentyOveryFifteen = twentyOveryFifteen
            };
            return View(viewModel);
        }

        public ActionResult UpTrends()
        {
            var twoOverTwo = _stockService.GetStockTrendReport(2, 2, "Bullish").ToList();
            var threeOverThree = _stockService.GetStockTrendReport(3, 3, "Bullish").ToList();
            var fiveOverFive = _stockService.GetStockTrendReport(5, 5, "Bullish").ToList();
            var tenOverEight = _stockService.GetStockTrendReport(10, 8, "Bullish").ToList();
            var twentyOverFifteen = _stockService.GetStockTrendReport(20, 15, "Bullish").ToList();
            var thirtyOverTwenty = _stockService.GetStockTrendReport(30, 20, "Bullish").ToList();

            var viewModel = new UpTrendReportViewModel
            {
                TwoOverTwo = twoOverTwo,
                ThreeOverThree = threeOverThree,
                FiveOverFive = fiveOverFive,
                TenOverEight = tenOverEight,
                ThirtyOverTwenty = thirtyOverTwenty,
                TwentyOveryFifteen = twentyOverFifteen
            };
            return View(viewModel);
        }

        public ActionResult SuspendedAndOnSale()
        {
            try
            {
                //var result = _predictionService();
            }
            catch (Exception ex)
            {

            }
            return null;
        }

    }
}