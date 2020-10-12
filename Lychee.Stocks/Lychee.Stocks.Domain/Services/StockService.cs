using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using LazyCache;
using Lychee.Caching.Interfaces;
using Lychee.CommonHelper.DictionaryExtensions;
using Lychee.Domain.Interfaces;
using Lychee.Infrastructure.Interfaces;
using Lychee.Stocks.Common.Interfaces;
using Lychee.Stocks.Domain.Constants;
using Lychee.Stocks.Domain.Helpers;
using Lychee.Stocks.Domain.Interfaces.Repositories;
using Lychee.Stocks.Domain.Interfaces.Services;
using Lychee.Stocks.Domain.Models;
using Lychee.Stocks.Entities;
using Lychee.Stocks.InvestagramsApi.Interfaces;
using Lychee.Stocks.InvestagramsApi.Models.Calendar;
using Lychee.Stocks.InvestagramsApi.Models.Stocks;
using Omu.ValueInjecter;
using Omu.ValueInjecter.Injections;
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
        private readonly IInvestagramsApiCachedService _investagramsApiService;
        private readonly ICookieProviderService _cookieProviderService;
        private readonly IStockScoreService _stockScoreService;
        private readonly IStockHistoryRepository _stockHistoryRepository;
        private readonly IStockMarketStatusRepository _stockMarketStatusRepository;
        private readonly ICandleStickAnalyzerService _candleStickAnalyzerService;
        private readonly IEventRepository _eventRepository;

        private const string CACHE_MORNING_STAR_DOJI = "MorningStarDoji-{0}";
        private const string CACHE_HAMMER = "Hammer-{0}";
        private const string CACHE_LATEST_STOCK_HISTORY = "LatestStockHistory-{0}";
        private const string CACHE_HIGHEST_TRADES = "HighestTrades-{0}";
        private const string CACHE_HIGHEST_VOLUMES = "HighestVolumes-{0}";

        public StockService(IDatabaseFactory databaseFactory, ISettingService settingService,
            IRepository<Stock> stockRepository,
            IStockHistoryRepository stockHistoryRepository, ICachingFactory cacheFactory,
            ICookieProviderService cookieProviderService, IInvestagramsApiCachedService investagramsApiService,
            IStockScoreService stockScoreService, IStockMarketStatusRepository stockMarketStatusRepository, ICandleStickAnalyzerService candleStickAnalyzerService, IEventRepository eventRepository)
        {
            _databaseFactory = databaseFactory;
            _settingService = settingService;
            _stockRepository = stockRepository;
            _stockHistoryRepository = stockHistoryRepository;
            _cookieProviderService = cookieProviderService;
            _investagramsApiService = investagramsApiService;
            _stockScoreService = stockScoreService;
            _stockMarketStatusRepository = stockMarketStatusRepository;
            _candleStickAnalyzerService = candleStickAnalyzerService;
            _eventRepository = eventRepository;
            _cache = cacheFactory.GetCacheService();
        }

        public void SaveLatestStockUpdate()
        {
            var stocks = new List<ScreenerResponse>();
            var realTimePrice = new List<RealTimePrice>();

            var events = new List<Event>();

            ClearCache();

            //Save stocks
            var tasks = new List<Task>
            {
                Task.Run(async () => await _investagramsApiService.GetAllLatestStocks()).ContinueWith(x =>
                {
                    stocks.AddRange(x.Result);
                    AddEvents(x.Result.OrderByDescending(e => e.Volume).Take(10), events, EventTypes.HighestVolume);
                }),
                Task.Run(async () => await _investagramsApiService.GetAllActiveStockPriceRealTime()).ContinueWith(x =>
                {
                    realTimePrice.AddRange(x.Result);
                    AddEvents(x.Result.OrderByDescending(e => e.Trades).Take(10), events, EventTypes.HighestTrade);
                }),
                
                //adding events
                Task.Run(async() => await _investagramsApiService.GetMarketStatus(DateTime.Now)).ContinueWith(x =>
                {
                    AddEvents(x.Result.MostActive, events, EventTypes.MostActive);
                    AddEvents(x.Result.TopGainer, events, EventTypes.TopWinner);
                    AddEvents(x.Result.TopLoser, events, EventTypes.TopLoser);
                }),

                Task.Run(async () => await _investagramsApiService.Get52WeekLow()).ContinueWith(x => AddEvents(x.Result, events, EventTypes._52WeekLow)),
                Task.Run(async () => await _investagramsApiService.GetOversoldStocks()).ContinueWith(x => AddEvents(x.Result, events, EventTypes.Oversold)),
                Task.Run(async () => await _investagramsApiService.GetTrendingStocks()).ContinueWith(x => AddEvents(x.Result, events, EventTypes.Trending)),
                Task.Run(async () => await _investagramsApiService.GreenVolume()).ContinueWith(x => AddEvents(x.Result, events, EventTypes.GreenVolume)),
            };
            Task.WaitAll(tasks.ToArray());

            if (stocks == null)
                throw new System.Exception("Please update investa cookie");

            SaveStocks(stocks, realTimePrice);
            SaveEvents(events);
        }

        private void AddEvents(IEnumerable<IStock> stockCodes, List<Event> events, string eventType)
        {
            if (stockCodes == null) return;

            stockCodes = stockCodes.ToList();
            var date = _stockMarketStatusRepository.GetLastTradingDate();

            events.AddRange(stockCodes.Select(e => new Event
            {
                StockCode = e.StockCode,
                Date = date,
                EventType = eventType
            }));
        }

        /// <summary>
        /// Only use this if you missed to fetched some trades on some day. This will not include the trades
        /// </summary>
        public async Task UpdateAllStock()
        {
            var allStocks = await _investagramsApiService.GetAllActiveStockPriceRealTime();
            var charts = new Dictionary<string, ChartHistory>();
            var tasks = new List<Task>();

            foreach (var stock in allStocks)
            {
                tasks.Add(Task.Run(async () => await _investagramsApiService.GetChartHistoryByDate(stock.StockId, DateTime.Now))
                    .ContinueWith(x => charts.Add(stock.StockCode, x.Result)));

            }

            Task.WaitAll(tasks.ToArray());

            foreach (var chartHistory in charts)
            {
                try
                {
                    var lastDate = chartHistory.Value.Dates.Last();
                    var allStockHistory = _stockHistoryRepository.Find(x => x.StockCode == chartHistory.Key && x.Date >= lastDate).ToList();

                    var index = 0;
                    foreach (var date in chartHistory.Value.Dates)
                    {
                        var stockHistory = allStockHistory.FirstOrDefault(x => x.Date == date);
                        if (stockHistory == null)
                        {
                            _stockHistoryRepository.Add(new StockHistory
                            {
                                StockCode = chartHistory.Key,
                                Last = chartHistory.Value.Closes[index],
                                Date = date,
                                Low = chartHistory.Value.Lows[index],
                                Open = chartHistory.Value.Opens[index],
                                High = chartHistory.Value.Highs[index],
                                Volume = chartHistory.Value.Volumes[index]
                            });
                        }

                        index++;
                    }

                    _stockHistoryRepository.SaveChanges();
                }
                catch (System.Exception ex)
                {
                    //carry on if something happens from upsert
                }
            }
        }

        private void SaveStocks(List<ScreenerResponse> stocks, List<RealTimePrice> realTimePrice)
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

        private void SaveEvents(List<Event> events)
        {
            var date = _stockMarketStatusRepository.GetLastTradingDate();
            var allEvents = _eventRepository.GetEvents(date);

            foreach (var ev in events)
            {
                if (allEvents != null && allEvents.ContainsKey((ev.StockCode, ev.EventType)))
                {
                    var item = allEvents.GetValue(ev.StockCode, ev.EventType);
                    _eventRepository.Attach(item);
                    item.InjectFrom(new LoopInjection(new[]
                    {
                        nameof(Event.EventId)
                    }),ev);
                }
                else
                {
                    _eventRepository.Add(ev);
                }
            }

            _eventRepository.SaveChanges();

            if (allEvents == null)
                _cache.Remove($"Events-{date:MMdd}");
        }

        public Dictionary<string, StockHistory> GetLatestStockHistory()
        {
            var date = _stockMarketStatusRepository.GetLastTradingDate();
            var cacheKey = string.Format(CACHE_LATEST_STOCK_HISTORY, date.ToString("MMdd"));
            return _cache.GetOrAdd(cacheKey, () => _stockHistoryRepository.RetrieveStockLastHistory().ToDictionary(x => x.StockCode, x => x));
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

        public List<StockHistory> GetTop10HighestTrades(DateTime? date = null)
        {
            if (date == null)
                date = _stockMarketStatusRepository.GetLastTradingDate();

            var cacheKey = string.Format(CACHE_HIGHEST_TRADES, $"{date: MMdd}");
            return _cache.GetOrAdd(cacheKey, () => _stockHistoryRepository.GetTop10HighesTrades(date.Value));
        }

        public List<StockHistory> GetTop10HighestVolumes(DateTime? date = null)
        {
            if (date == null)
                date = _stockMarketStatusRepository.GetLastTradingDate();

            var cacheKey = string.Format(CACHE_HIGHEST_VOLUMES, $"{date: MMdd}");
            return _cache.GetOrAdd(cacheKey, () => _stockHistoryRepository.GetTop10HighesVolumes(date.Value));
        }

        public List<StockHistory> GetStockWithSteepDown()
        {
            var date = _stockMarketStatusRepository.GetLastTradingDate();
            var allStocks = GetTopXTradeHistory(date, 30);
            var stocksWithRedCandle = allStocks.Where(x => x.Date == date && x.Last < x.Open).Select(x => x.StockCode).ToList();
            var stocksWithSteepDown = new List<StockHistory>();

            foreach (var code in stocksWithRedCandle)
            {
                var stocks = allStocks.Where(x => x.StockCode == code).ToList();
                var candles = stocks.Select(x =>
                {
                    var item = Mapper.Map<CandleStick>(x);
                    item.Close = x.Last;

                    return item;
                }).ToList();
                
                var lastCandle = candles.First();
                var biggestCandle = candles.OrderByDescending(x => x.CandleBodyHeight).First();

                if (lastCandle.CandleFullHeight == biggestCandle.CandleFullHeight)
                    stocksWithSteepDown.Add(stocks.First());
            }

            return stocksWithSteepDown;
        }

        public List<StockTradeAverage> GetStockTradeAverages(int averageDays, int averageTrades)
        {
            var date = _stockMarketStatusRepository.GetLastTradingDate();
            var cacheKey = $"StockTradeAverage-{date:MMdd}-{averageDays}-{averageTrades}";
            return _cache.GetOrAdd(cacheKey, () => _stockHistoryRepository.GetAverageStocks(averageDays, averageTrades));
        }

        public List<StockHistory> GetTopXTradeHistory(DateTime date, int topX)
        {
            var cacheKey = $"TopXTradeHistory-{date:MMdd}-{topX}";
            return _cache.GetOrAdd(cacheKey, () => _stockHistoryRepository.GetTopXTradeHistory(date, topX));
        }

        public List<StockHistory> GetMorningStarDoji()
        {
            var date = _stockMarketStatusRepository.GetLastTradingDate();
            var cacheKey = string.Format(CACHE_MORNING_STAR_DOJI, date.ToString("MMdd"));

            var dojiStocks = _cache.GetOrAdd(cacheKey, () => _stockHistoryRepository.GetAllDojis());
            var allStocks = GetTopXTradeHistory(date, 30);
            var morningStarDojiStocks = new List<StockHistory>();

            foreach (var dojiStock in dojiStocks)
            {
                var chartHistory = MapToChartHistory(allStocks.Where(x => x.StockCode == dojiStock.StockCode).ToList());
                if (_candleStickAnalyzerService.IsMorningStarDoji(chartHistory))
                    morningStarDojiStocks.Add(dojiStock);
            }

            return morningStarDojiStocks;
        }

        public List<StockHistory> GetHammers()
        {
            var date = _stockMarketStatusRepository.GetLastTradingDate();
            var cacheKey = string.Format(CACHE_HAMMER, date.ToString("MMdd"));

            var hammers = _cache.GetOrAdd(cacheKey, () => _stockHistoryRepository.GetAllHammers());
            var allStocks = GetTopXTradeHistory(date, 30);
            var hammerStocks = new List<StockHistory>();

            foreach (var dojiStock in hammers)
            {
                var chartHistory = MapToChartHistory(allStocks.Where(x => x.StockCode == dojiStock.StockCode).ToList());
                if (_candleStickAnalyzerService.IsHammer(chartHistory))
                    hammerStocks.Add(dojiStock);
            }

            return hammerStocks;
        }

        private void ClearHighestTradesCache(DateTime? date = null)
        {
            if (date == null)
                date = _stockMarketStatusRepository.GetLastTradingDate();

            var cacheKey = string.Format(CACHE_HIGHEST_TRADES, $"{date: MMdd}");
            _cache.SafeRemove<List<StockHistory>>(cacheKey);
        }

        private void ClearStockTradeAverageCache()
        {
            var date = _stockMarketStatusRepository.GetLastTradingDate();
            var cacheKey = $"StockTradeAverage-{date:MMdd}-{2}-{100}";

            _cache.SafeRemove<List<StockHistory>>(cacheKey);
        }

        private void ClearTopXTradeHistoryCache()
        {
            var date = _stockMarketStatusRepository.GetLastTradingDate();
            var cacheKey = $"TopXTradeHistory-{date:MMdd}-{30}";

            _cache.SafeRemove<List<StockHistory>>(cacheKey);
        }

        private void ClearMorningStarDojiCache()
        {
            var date = _stockMarketStatusRepository.GetLastTradingDate();
            var cacheKey = string.Format(CACHE_MORNING_STAR_DOJI, date.ToString("MMdd"));
            _cache.SafeRemove<List<StockHistory>>(cacheKey);
        }

        private void ClearHammers()
        {
            var date = _stockMarketStatusRepository.GetLastTradingDate();
            var cacheKey = string.Format(CACHE_HAMMER, date.ToString("MMdd"));
            _cache.SafeRemove<List<StockHistory>>(cacheKey);
        }

        private void ClearLatestStockHistory()
        {
            var date = _stockMarketStatusRepository.GetLastTradingDate();
            var cacheKey = string.Format(CACHE_LATEST_STOCK_HISTORY, date.ToString("MMdd"));
            _cache.SafeRemove<List<StockHistory>>(cacheKey);
        }

        private void ClearCache()
        {
            ClearStockTradeAverageCache();
            ClearTopXTradeHistoryCache();
            ClearMorningStarDojiCache();
            ClearHammers();
            ClearLatestStockHistory();
            ClearHighestTradesCache();

            _investagramsApiService.ClearAllCache();
        }

        private ChartHistory MapToChartHistory(List<StockHistory> stockHistory)
        {
            var chartHistory = new ChartHistory();
            chartHistory.Opens = stockHistory.OrderByDescending(x => x.Date).Select(x => x.Open).ToArray();
            chartHistory.Closes = stockHistory.OrderByDescending(x => x.Date).Select(x => x.Last).ToArray();
            chartHistory.Highs = stockHistory.OrderByDescending(x => x.Date).Select(x => x.High).ToArray();
            chartHistory.Lows = stockHistory.OrderByDescending(x => x.Date).Select(x => x.Low).ToArray();
            chartHistory.Dates = stockHistory.OrderByDescending(x => x.Date).Select(x => x.Date).ToArray();

            return chartHistory;
        }

    }
}

