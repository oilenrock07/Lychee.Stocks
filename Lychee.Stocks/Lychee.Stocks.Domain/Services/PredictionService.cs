using System;
using System.Collections.Generic;
using System.Linq;
using Lychee.Infrastructure.Interfaces;
using Lychee.Stocks.Domain.Interfaces.Services;
using Lychee.Stocks.Entities;

namespace Lychee.Stocks.Domain.Services
{
    public class PredictionService : IPredictionService
    {
        private readonly IRepository<MyPrediction> _predictionRepository;

        public PredictionService(IRepository<MyPrediction> predictionRepository)
        {
            _predictionRepository = predictionRepository;
        }

        public ICollection<MyPrediction> GetLast5DaysPredictions()
        {
            var date = DateTime.Now.AddDays(-5);
            return _predictionRepository
                .Find(x => x.DateCreated >= date)
                .OrderByDescending(x => x.DateCreated)
                .ToList();
        }
    }
}
