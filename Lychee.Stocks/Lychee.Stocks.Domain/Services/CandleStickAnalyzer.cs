using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lychee.Stocks.Domain.Enums;
using Lychee.Stocks.Domain.Interfaces.Services;
using Lychee.Stocks.Domain.Models;
using Lychee.Stocks.InvestagramsApi.Models.Stocks;

namespace Lychee.Stocks.Domain.Services
{
    public class CandleStickAnalyzerService : ICandleStickAnalyzerService
    {
        private readonly decimal _dojiRangePercentage = 0.1m;

        public CandleStickReversalPattern GetCandleStickPattern(ChartHistory history)
        {
            if (IsEveningStarDoji(history))
                return CandleStickReversalPattern.EveningStarDoji;

            return CandleStickReversalPattern.None;
        }

        public bool IsEveningStarDoji(ChartHistory history)
        {
            var candle = GetCandleStick(history, 0);
            var isDoji = IsDoji(candle);

            if (isDoji)
            {
                var totalPreviousCandleHeight = GetAllGreenCandleTotalHeight(history, 1);
                var getCandlesCount = history.Dates.Length > 15 ? 15 : history.Dates.Length;
                var last15Candles = GetAllCandleSticks(history, 1, getCandlesCount);
                var lastCandleClose = last15Candles.First().Close;

                //if the accumulated up recently is atleast 3rd of the height bar in 15days then return true
                if (getCandlesCount > 3)
                {
                    var isWithinTop3Candles = totalPreviousCandleHeight >= last15Candles
                        .OrderByDescending(x => x.CandleBodyHeight).ToList()
                        .ElementAt(2)
                        .CandleBodyHeight;

                    if (isWithinTop3Candles && candle.Close >= lastCandleClose)
                        return true;
                }

                
                if (candle.Close >= lastCandleClose * 0.8m)
                    return true;
            }
            
            return false;
        }

        public bool IsMorningStarDoji(ChartHistory history)
        {
            var candle = GetCandleStick(history, 0);
            var isDoji = IsDoji(candle);

            if (isDoji)
            {
                var totalPreviousCandleHeight = GetAllRedCandleTotalHeight(history, 1);
                var getCandlesCount = history.Dates.Length > 15 ? 15 : history.Dates.Length;
                var last15Candles = GetAllCandleSticks(history, 1, getCandlesCount - 1);
                var lastCandleClose = last15Candles.First().Close;

                //if the accumulated up recently is atleast 3rd of the height bar in 15days then return true
                if (getCandlesCount > 3)
                {
                    var isWithinTop3Candles = totalPreviousCandleHeight >= last15Candles
                        .OrderByDescending(x => x.CandleBodyHeight).ToList()
                        .ElementAt(2)
                        .CandleBodyHeight;

                    if (isWithinTop3Candles && candle.Close <= lastCandleClose)
                        return true;
                }

                if (candle.Close <= lastCandleClose * 0.8m)
                    return true;
            }

            return false;
        }


        /// <summary>
        /// Loop all the previous candle until not green and get the total height
        /// </summary>
        /// <returns></returns>
        private decimal GetAllGreenCandleTotalHeight(ChartHistory history, int start)
        {
            var allGreenCandlesHeight = 0m;
            var stop = false;
            var ctr = start;

            do
            {
                var candle = GetCandleStick(history, ctr);
                if (candle.IsRedCandle)
                    stop = true;
                else
                {
                    allGreenCandlesHeight += candle.CandleBodyHeight;
                    ctr++;
                }

            } while (!stop);

            return allGreenCandlesHeight;
        }

        /// <summary>
        /// Loop all the previous candle until not red and get the total height
        /// </summary>
        /// <returns></returns>
        private decimal GetAllRedCandleTotalHeight(ChartHistory history, int start)
        {
            var allRedCandlesHeight = 0m;
            var stop = false;
            var ctr = start;

            do
            {
                var candle = GetCandleStick(history, ctr);
                if (candle.IsGreenCandle)
                    stop = true;
                else
                {
                    allRedCandlesHeight += candle.CandleBodyHeight;
                    ctr++;
                }

            } while (!stop);

            return allRedCandlesHeight;
        }

        private List<CandleStick> GetAllCandleSticks(ChartHistory history, int start, int count)
        {
            var candles = new List<CandleStick>();
            var ctr = start;
            while (ctr < start + count)
            {
                candles.Add(GetCandleStick(history, ctr));
                ctr++;
            }

            return candles;
        }

        public bool IsDoji(CandleStick candle)
        {
            var boundary = candle.CandleFullHeight * _dojiRangePercentage;
            return candle.Close <= Math.Round(candle.Open + boundary, 2) && candle.Close >= Math.Round(candle.Open - boundary, 2);
        }

        public CandleStick GetCandleStick(ChartHistory history, int index)
        {
            var candleStick = new CandleStick
            {
                Open = history.Opens[index],
                Low = history.Lows[index],
                High = history.Highs[index],
                Close = history.Closes[index]
            };

            return candleStick;
        }
    }
}
