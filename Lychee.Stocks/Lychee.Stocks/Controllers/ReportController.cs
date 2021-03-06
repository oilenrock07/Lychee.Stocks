﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Lychee.Stocks.Domain.Interfaces.Services;
using Lychee.Stocks.InvestagramsApi.Interfaces;
using Lychee.Stocks.InvestagramsApi.Models.Stocks;
using Lychee.Stocks.Models.Reports;
using Omu.ValueInjecter;

namespace Lychee.Stocks.Controllers
{
    public class ReportController : Controller
    {
        private readonly IStockService _stockService;
        private readonly IPredictionService _predictionService;
        private readonly IInvestagramsApiCachedService _investagramsApi;

        public ReportController(IStockService stockService, IPredictionService predictionService,
            IInvestagramsApiCachedService investagramsApi)
        {
            _stockService = stockService;
            _predictionService = predictionService;
            _investagramsApi = investagramsApi;
        }

        public async Task<ActionResult> TrendingNow()
        {
            var model = await _investagramsApi.GetTrendingStocks();
            return View(model);
        }

        public async Task<ActionResult> MacdAboutToCrossFromBelowBullish()
        {
            var model = await _investagramsApi.GetMacdAboutToCrossFromBelowBullish();
            return View(model);
        }

        public async Task<ActionResult> MacdCrossingSignalFromBelowBullish()
        {
            var model = await _investagramsApi.GetMacdCrossingSignalFromBelowBullish();
            return View(model);
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

        public async Task<ActionResult> SuspendedAndOnSale()
        {
            var model = await _investagramsApi.GetLatestStockMarketActivity();
            return View(model);
        }

        public async Task<ActionResult> MarketStatus()
        {
            var status = await _investagramsApi.GetMarketStatus(DateTime.Now);
            var viewModel = new MarketStatusViewModel
            {
                TopGainer = status.TopGainer.Select(x => MapMarketStatusTopGainers(x, status.MostActive)).ToArray(),
                TopLoser = status.TopLoser.Select(x => MapMarketStatusTopGainers(x, status.MostActive)).ToArray(),
                MostActive = status.MostActive.Select(x => MapMarketStatusMostActive(x, status)).ToArray()
            };

            return View(viewModel);
        }

        public async Task<ActionResult> OversoldStocks()
        {
            var stocks = await _investagramsApi.GetOversoldStocks();
            return View(stocks);
        }

        public async Task<ActionResult> OversoldStocksBelow20()
        {
            var stocks = await _investagramsApi.GetOversoldStocksLessThan20();
            return View(stocks);
        }

        private MarketStatusItemViewModel MapMarketStatusTopGainers(MarketStatusModel source, MarketStatusModel[] arr)
        {
            var item = Mapper.Map<MarketStatusItemViewModel>(source);
            item.Badge = arr.Any(a => a.StockCode == source.StockCode) ? "most-active" : string.Empty;
            item.BadgeClass = !string.IsNullOrEmpty(item.Badge) ? "default" : string.Empty;
            return item;
        }

        private MarketStatusItemViewModel MapMarketStatusMostActive(MarketStatusModel source, MarketStatus status)
        {
            var item = Mapper.Map<MarketStatusItemViewModel>(source);
            item.Badge = status.TopGainer.Any(a => a.StockCode == source.StockCode) ? "top-gainer" : string.Empty;
            item.BadgeClass = !string.IsNullOrEmpty(item.Badge) ? "success" : string.Empty;

            if (string.IsNullOrEmpty(item.Badge))
            {
                item.Badge = status.TopLoser.Any(a => a.StockCode == source.StockCode) ? "top-loser" : string.Empty;
                item.BadgeClass = !string.IsNullOrEmpty(item.Badge) ? "danger" : string.Empty;
            }
                

            return item;
        }

    }
}