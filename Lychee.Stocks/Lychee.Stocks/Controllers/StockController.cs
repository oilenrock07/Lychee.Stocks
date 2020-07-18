using System;
using System.Net.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using Lychee.Infrastructure.Interfaces;
using Lychee.Stocks.Domain.Interfaces.Services;
using Lychee.Stocks.Entities;
using Lychee.Stocks.Models;
using Microsoft.Ajax.Utilities;

namespace Lychee.Stocks.Controllers
{
    public class StockController : Controller
    {
        private readonly IRepository<MyPrediction> _predictionRepository;
        private readonly IPredictionService _predictionService;
        private readonly IStockService _stockService;

        public StockController(IStockService stockService, IRepository<MyPrediction> predictionRepository, IPredictionService predictionService)
        {
            _stockService = stockService;
            _predictionRepository = predictionRepository;
            _predictionService = predictionService;
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

        public async Task<ActionResult> GetEodStockUpdate()
        {
            return View();
        }

        //[OutputCache(Duration = 300)] //5 minutes cache
        public PartialViewResult Prediction()
        {
            var predictions = _predictionService.GetLast5DaysPredictions();
            return PartialView(predictions);
        }

        public ActionResult EditPrediction(int id)
        {
            var prediction = _predictionRepository.GetById(id);
            return View(prediction);
        }

        [HttpPost]
        public ActionResult EditPrediction(MyPrediction model)
        {
            _predictionRepository.Update(model);
            _predictionRepository.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult CreatePrediction()
        {
            return View(new MyPrediction());
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


        [HttpPost]
        public ActionResult CreatePrediction(MyPrediction model)
        {
            model.DateCreated = DateTime.Now;
            _predictionRepository.Add(model);
            _predictionRepository.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}