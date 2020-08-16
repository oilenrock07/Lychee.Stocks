using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using LazyCache;
using Lychee.Caching.Interfaces;
using Lychee.CommonHelper.Extensions;
using Lychee.Domain.Interfaces;
using Lychee.Infrastructure.Interfaces;
using Lychee.Stocks.Domain.Constants;
using Lychee.Stocks.Domain.Interfaces.Repositories;
using Lychee.Stocks.Domain.Interfaces.Services;
using Lychee.Stocks.Domain.Models;
using Lychee.Stocks.Entities;
using Lychee.Stocks.InvestagramsApi.Interfaces;
using Lychee.Stocks.InvestagramsApi.Models.Calendar;
using Lychee.Stocks.InvestagramsApi.Models.Stocks;
using Omu.ValueInjecter;
using Serilog;
using SuspendedStock = Lychee.Stocks.InvestagramsApi.Models.Stocks.SuspendedStock;

namespace Lychee.Stocks.Domain.Services
{
    public class StockService : IStockService
    {
        private readonly IDatabaseFactory _databaseFactory;
        private readonly ISettingService _settingService;
        private readonly IRepository<Stock> _stockRepository;
        private readonly IAppCache _cache;
        private readonly ISuspendedStockRepository _suspendedStockRepository;
        private readonly IBlockSaleStockRepository _blockSaleStockRepository;
        private readonly IInvestagramsApiCachedService _investagramsApiService;
        private readonly ICookieProviderService _cookieProviderService;
        private readonly IStockScoreService _stockScoreService;
        private readonly IStockHistoryRepository _stockHistoryRepository;
        private readonly IStockMarketStatusRepository _stockMarketStatusRepository;


        public StockService(IDatabaseFactory databaseFactory, ISettingService settingService,
            IRepository<Stock> stockRepository,
            IStockHistoryRepository stockHistoryRepository, ICachingFactory cacheFactory,
            ISuspendedStockRepository suspendedStockRepository, IBlockSaleStockRepository blockSaleStockRepository,
            ICookieProviderService cookieProviderService, IInvestagramsApiCachedService investagramsApiService,
            IStockScoreService stockScoreService, IStockMarketStatusRepository stockMarketStatusRepository)
        {
            _databaseFactory = databaseFactory;
            _settingService = settingService;
            _stockRepository = stockRepository;
            _stockHistoryRepository = stockHistoryRepository;
            _suspendedStockRepository = suspendedStockRepository;
            _blockSaleStockRepository = blockSaleStockRepository;
            _cookieProviderService = cookieProviderService;
            _investagramsApiService = investagramsApiService;
            _stockScoreService = stockScoreService;
            _stockMarketStatusRepository = stockMarketStatusRepository;
            _cache = cacheFactory.GetCacheService();
        }

        public async Task SaveLatestStockUpdate()
        {
            var stocks = new List<ScreenerResponse>();
            var realTimePrice = new List<RealTimePrice>();

            //Save stocks
            var tasks = new List<Task>
            {
                Task.Run(async () => await _investagramsApiService.GetAllLatestStocks()).ContinueWith(x => stocks.AddRange(x.Result)),
                Task.Run(async () => await _investagramsApiService.GetAllActiveStockPriceRealTime().ContinueWith(x => realTimePrice.AddRange(x.Result)))
            };
            Task.WaitAll(tasks.ToArray());

            if (stocks == null)
                throw new System.Exception("Please update investa cookie");

            SaveStocks(stocks, realTimePrice);
        }

        protected void SaveStocks(List<ScreenerResponse> stocks, List<RealTimePrice> realTimePrice)
        {
            var date = _stockMarketStatusRepository.GetLastTradingDate();
            var allStocks = _stockHistoryRepository.GetAllStocksByDate(date);

            foreach (var item in stocks)
            {
                var stock = allStocks?.FirstOrDefault(x => x.StockCode == item.StockCode && x.Date == date);
                var realTimeData = realTimePrice.FirstOrDefault(x => x.StockCode == item.StockCode);

                if (stock == null)
                {
                    var newStockHistory = Mapper.Map<StockHistory>(item);
                    newStockHistory.Date = date;
                    MapRealTimeDataToStockHistory(realTimeData, newStockHistory);

                    _stockHistoryRepository.Add(newStockHistory);
                }
                else
                {
                    _stockHistoryRepository.Attach(stock);
                    stock.InjectFrom(item);
                    MapRealTimeDataToStockHistory(realTimeData, stock);
                }
            }

            _databaseFactory.SaveChanges();
        }

        private void MapRealTimeDataToStockHistory(RealTimePrice realTimePrice, StockHistory stock)
        {
            if (realTimePrice == null) return;

            stock.Value = realTimePrice.Value;
            stock.Trades = realTimePrice.Trades;
        }

        public DateTime GetLastDataUpdates()
        {
            var lastStockHistory = _stockHistoryRepository.GetAll().Take(1).First();
            return lastStockHistory.Date;
        }

        public ICollection<StockTrendReportModel> GetStockTrendReport(int days, int losingWinningStreak, string trend = "Bearish")
        {
            var paramDays = new SqlParameter {ParameterName = "Days", Value = days};
            var paramLosingWinningStreak = new SqlParameter { ParameterName = "LosingWinningStreak", Value = losingWinningStreak };
            var paramTrend = new SqlParameter { ParameterName = "Trend", Value = trend };

            return _stockRepository.ExecuteSqlQuery<StockTrendReportModel>(
                "EXEC RetrieveStockTrendReport @Days, @LosingWinningStreak, @Trend", paramDays,
                paramLosingWinningStreak, paramTrend).ToList();
        }

        public Task<ICollection<SuspendedStock>> GetSuspendedStocks()
        {
            throw new NotImplementedException();
        }


        public virtual async Task<LatestStockMarketActivityVm> GetSuspendedAndBlockSaleStocks()
        {
            return await _investagramsApiService.GetLatestStockMarketActivity();
        }


        public virtual async Task UpdateSuspendedStocks()
        {
            //var suspendedStocks = await GetSuspendedStocks();
            //if (!suspendedStocks.Any())
            //    return;

            //var lastSuspensionDate = _suspendedStockRepository.GetLastStockSuspensionDate();

            //if (lastSuspensionDate != suspendedStocks.First().Date)
            //    _suspendedStockRepository.SaveSuspendedStocks(suspendedStocks);
        }

        public virtual async Task<ICollection<StockBlockSale>> GetBlockSaleStocks()
        {
            //var data = _cache.Get<LatestStockMarketActivityVm>(CacheNames.StockMarketActivityVm);
            //if (data != null)
            //    return data.StockBlockSaleList;


            //var result = await _investagramsApiRepository.GetLatestStockMarketActivity();
            //var blockSaleList = result.StockBlockSaleList.ToList();

            //_cache.Add(CacheNames.StockMarketActivityVm, result, TimeSpan.FromHours(12));

            //return blockSaleList;
            return null;
        }

        /// <summary>
        /// Block Sales are those stocks that someone might buy a lot. This is a good indicator that it may go up
        /// </summary>
        public virtual async Task UpdateBlockSaleStocks()
        {
            //var blockSaleStocks = await GetBlockSaleStocks();
            //if (!blockSaleStocks.Any())
            //    return;

            //var lastBlockSaleDate = _blockSaleStockRepository.GetLastBlockSaleStocksDate();

            //if (lastBlockSaleDate != blockSaleStocks.First().Date)
                //_blockSaleStockRepository.SaveBlockSaleStocks(blockSaleStocks);
        }

        public virtual async Task UpdateStocks(IEnumerable<string> stockCodes)
        {
            //var stockList = new List<ViewStock>();
            //var suspendedStocksToday = new List<string>();

            ////todo: exclude the suspended stocks from getting details in api

            //var tasks = new List<Task<ViewStock>>();
            //foreach (var code in stockCodes)
            //{
            //    tasks.Add(_investagramsApiRepository.ViewStock(code));
            //}

            //await Task.WhenAll(tasks);
            //stockList = tasks.Select(x => x.Result).ToList();
        }

        public async Task<StockScore> GetStockTotalScore(string stockCode)
        {
            var score = new StockScore();

            try
            {
                var stock = await _investagramsApiService.ViewStockWithoutFundamentalAnalysis(stockCode);
                score.StockId = stock.StockInfo.StockId;

                score.AddReasons(_stockScoreService.GetBreakingResistanceScore(stock));
                score.AddReasons(_stockScoreService.GetBreakingSupport2Score(stock));
                score.AddReasons(_stockScoreService.GetTradeScore(stock));
                score.AddReasons(_stockScoreService.GetMa9Score(stock));
                score.AddReasons(_stockScoreService.GetMa20Score(stock));
                score.AddReasons(_stockScoreService.GetRsiScore(stock));
                score.AddReasons(_stockScoreService.GetVolume15Score(stock));

                var tasks = new List<Task>
                {
                    Task.Run(async () => await _stockScoreService.GetBidAndAskScore(stock)).ContinueWith(x => score.AddReasons(x.Result)),
                    Task.Run(async () => await _stockScoreService.GetRecentlySuspendedAndBlockSaleScore(stockCode)).ContinueWith(x => score.AddReasons(x.Result)),
                    Task.Run(async () => await _stockScoreService.GetTrendingStockScore(stockCode)).ContinueWith(x => score.AddReasons(x.Result)),
                    Task.Run(async () => await _stockScoreService.GetMostActiveAndGainerScore(stockCode)).ContinueWith(x => score.AddReasons(x.Result)),
                    Task.Run(async () => await _stockScoreService.GetDividendScore(stockCode)).ContinueWith(x => score.AddReasons(x.Result)),
                    Task.Run(async () => await _stockScoreService.GetMacdAboutToCrossFromBelowBullishScore(stockCode)).ContinueWith(x => score.AddReasons(x.Result)),
                    Task.Run(async () => await _stockScoreService.GetMacdCrossingSignalFromBelowBullishScore(stockCode)).ContinueWith(x => score.AddReasons(x.Result)),

                    Task.Run(async () => await _investagramsApiService.GetChartHistoryByDate(stock.StockInfo.StockId, DateTime.Now))
                        .ContinueWith(x => score.ChartHistory = x.Result)
                };
                Task.WaitAll(tasks.ToArray());
            }
            catch (System.Exception ex)
            {
                Log.Logger.Error($"{nameof(GetStockTotalScore)} StockCode:{stockCode} Exception:{ex}");
            }
            
            //2/2 winner
            //8/10 loser 

            //Reached cap/max buying - matic 100 %
            //has a really steep down recently 20%
            //dormant volume then suddenly spiked
            //candle stick movement.
            //trades to be average 20 not fixed to 1500
            return score;
        }

        public async Task<List<Dividend>> GetStocksGivingDividends()
        {
            var calendar = await _investagramsApiService.GetCalendarOverview();
            return calendar.Dividends.ToList();
        }

        public async Task<StockAnalysisModel> AnalyzeStock(string stockCode)
        {
            var model = new StockAnalysisModel { StockCode = stockCode};
            var score = await GetStockTotalScore(stockCode);

            var passingScore = _settingService.GetSettingValue<decimal>(SettingNames.Score_ShouldIBuyStockPassingScore);
            model.ShouldIBuyStock = score.TotalScore >= passingScore || score.HasSignificantUptrendReason ? "Yes" : "No";
            model.UpTrendReasons = score.UpTrendReasons;
            model.DownTrendReasons = score.DownTrendReasons;
            model.TotalScore = score.TotalScore;
            model.StockId = score.StockId;
            model.ChartHistory = Utf8Json.JsonSerializer.ToJsonString(score.ChartHistory);

            return model;
        }

        public async Task<List<StockAnalysisModel>> AnalyzeTrendingStock()
        {
            var trendingStocks = await _investagramsApiService.GetTrendingStocks();
            var result = new List<StockAnalysisModel>();

            //todo:make an experiment here if making in a normal loop is faster than making it a parallel tasks. Record the time it takes
            var tasks = new List<Task>();
            foreach (var trendingStock in trendingStocks)
            {
                tasks.Add(Task.Run(async () => await AnalyzeStock(trendingStock.Stock.StockCode)).ContinueWith(x => result.Add(x.Result)));
            }

            Task.WaitAll(tasks.ToArray());
            return result;
        }

        public void UpdateInvestagramsCookie(string value)
        {
            _cookieProviderService.SetCookie(value);
        }

        public List<StockHistory> GetStockWithSteepDown()
        {
            return _stockHistoryRepository.GetAllStocksWithSteepDown();
        }

        private void UpdateBlockSale(ICollection<StockBlockSale> stockBlockSales)
        {
            if (!stockBlockSales.Any())
                return;

            var date = stockBlockSales.First().Date;
            var lastSuspendedDate = _blockSaleStockRepository.GetLastBlockSaleStocksDate();

            if (date.Date != lastSuspendedDate?.Date)
                _blockSaleStockRepository.SaveBlockSaleStocks(stockBlockSales);
        }

        private void UpdateSuspendedStocks(ICollection<InvestagramsApi.Models.Stocks.SuspendedStock> suspendedStocks)
        {
            if (!suspendedStocks.Any())
                return;

            var date = suspendedStocks.First().Date;
            var lastSuspendedDate = _suspendedStockRepository.GetLastStockSuspensionDate();

            if (date.Date != lastSuspendedDate?.Date)
                _suspendedStockRepository.SaveSuspendedStocks(suspendedStocks);
        }


    }
}

