using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lychee.Stocks.Domain.Models;

namespace Lychee.Stocks.Domain.Interfaces.Services
{
    public interface ICandleStickAnalyzerService
    {
        bool IsDoji(CandleStick candle);
    }
}
