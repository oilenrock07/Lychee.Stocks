using System.Web.Mvc;
using Lychee.Infrastructure.Interfaces;
using Lychee.Stocks.Domain.Interfaces.Services;
using Lychee.Stocks.Entities;

namespace Lychee.Stocks.Controllers
{
    public class StockController : Controller
    {
        private readonly IRepository<MyPrediction> _predictionRepository;
        private readonly IStockService _stockService;

        public StockController(IStockService stockService, IRepository<MyPrediction> predictionRepository)
        {
            _stockService = stockService;
            _predictionRepository = predictionRepository;
        }

        //[OutputCache(Duration = 300)] //5 minutes cache
        public PartialViewResult Prediction()
        {
            var predictions = _stockService.GetLast5DaysPredictions();
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
    }
}