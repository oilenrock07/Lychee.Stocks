using System.Collections.Generic;
using Lychee.Stocks.Entities;

namespace Lychee.Stocks.Domain.Interfaces.Services
{
    public interface IPredictionService
    {
        ICollection<MyPrediction> GetLast5DaysPredictions();
    }
}
