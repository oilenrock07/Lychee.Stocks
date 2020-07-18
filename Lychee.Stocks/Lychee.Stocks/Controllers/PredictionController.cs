using System;
using System.Web.Mvc;
using Lychee.Infrastructure.Interfaces;
using Lychee.Stocks.Domain.Interfaces.Services;
using Lychee.Stocks.Entities;

namespace Lychee.Stocks.Controllers
{
    public class PredictionController : Controller
    {
        private readonly IRepository<MyPrediction> _predictionRepository;
        private readonly IPredictionService _predictionService;

        public PredictionController(IPredictionService predictionService, IRepository<MyPrediction> predictionRepository)
        {
            _predictionService = predictionService;
            _predictionRepository = predictionRepository;
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