using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using LazyCache;
using Lychee.Caching.Interfaces;
using Lychee.Domain.Interfaces;
using Lychee.Infrastructure.Interfaces;
using Lychee.Stocks.Domain.Constants;
using Lychee.Stocks.Domain.Interfaces.Repositories;
using Lychee.Stocks.Domain.Interfaces.Services;
using Lychee.Stocks.Entities;
using Lychee.Stocks.InvestagramsApi.Interfaces;
using Lychee.Stocks.InvestagramsApi.Models.Stocks;
using Omu.ValueInjecter;

namespace Lychee.Stocks.Domain.Services
{
    public class StockService : IStockService
    {
        private readonly IDatabaseFactory _databaseFactory;
        private readonly ISettingRepository _settingRepository;
        private readonly IRepository<Stock> _stockRepository;
        private readonly IRepository<TechnicalAnalysis> _technicalAnalysis;
        private readonly IAppCache _cache;
        private readonly ISuspendedStockRepository _suspendedStockRepository;
        private readonly IBlockSaleStockRepository _blockSaleStockRepository;
        private readonly IInvestagramsApiService _investagramsApiService;
        private readonly ICookieProviderService _cookieProviderService;
        private readonly IStockScoreService _stockScoreService;
        private readonly IStockHistoryRepository _stockHistoryRepository;


        private readonly string _investaCookie = "cache:investa-cookie";

        public StockService(IDatabaseFactory databaseFactory, ISettingRepository settingRepository,
            IRepository<Stock> stockRepository,
            IRepository<TechnicalAnalysis> technicalAnalysis,
            IStockHistoryRepository stockHistoryRepository, ICachingFactory cacheFactory, ISuspendedStockRepository suspendedStockRepository, IBlockSaleStockRepository blockSaleStockRepository, ICookieProviderService cookieProviderService, IInvestagramsApiService investagramsApiService, IStockScoreService stockScoreService)
        {
            _databaseFactory = databaseFactory;
            _settingRepository = settingRepository;
            _stockRepository = stockRepository;
            _technicalAnalysis = technicalAnalysis;
            _stockHistoryRepository = stockHistoryRepository;
            _suspendedStockRepository = suspendedStockRepository;
            _blockSaleStockRepository = blockSaleStockRepository;
            _cookieProviderService = cookieProviderService;
            _investagramsApiService = investagramsApiService;
            _stockScoreService = stockScoreService;
            _cache = cacheFactory.GetCacheService();
        }

        public async Task SaveLatestStockUpdate()
        {
            //Save stocks
            var stocks = await _investagramsApiService.GetAllLatestStocks();
            if (stocks == null)
                throw new System.Exception("Please update investa cookie");

            SaveStocks(stocks);
        }

        protected void SaveStocks(List<ScreenerResponse> stocks)
        {
            var date = GetLastTradingDate();
            var allStocks = _stockHistoryRepository.GetAllStocksByDate(date);

            foreach (var item in stocks)
            {
                var stock = allStocks?.FirstOrDefault(x => x.StockCode == item.StockCode && x.Date == date);
                if (stock == null)
                {
                    var newStockHistory = Mapper.Map<StockHistory>(item);
                    newStockHistory.Date = date;
                    _stockHistoryRepository.Add(newStockHistory);
                }
                else
                {
                    stock.InjectFrom(item);
                    _stockHistoryRepository.Update(stock);
                }
            }

            _databaseFactory.SaveChanges();
        }

        public bool HasStockData(DateTime date)
        {
            return false;
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


        public virtual async Task<ICollection<InvestagramsApi.Models.Stocks.SuspendedStock>> GetSuspendedStocks()
        {
            //var data = _cache.Get<LatestStockMarketActivityVm>(CacheNames.StockMarketActivityVm);
            //if (data != null)
            //    return data.StockSuspensionList;

            
            //var result = await _investagramsApiRepository.GetLatestStockMarketActivity();
            //result.SetSuspendedStockDate();
            //var stockSuspensionList = result.StockSuspensionList.ToList();

            //_cache.Add(CacheNames.StockMarketActivityVm, result, TimeSpan.FromHours(12));

            //return stockSuspensionList;
            return null;
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

        public async Task ShouldIBuyStock(string stockCode)
        {
            decimal totalScore = 0;

            var stock = await _investagramsApiService.ViewStockWithoutFundamentalAnalysis(stockCode);
            var chartHistory = await _investagramsApiService.GetChartHistoryByDate(stock.StockInfo.StockId, DateTime.Now);

            totalScore += _stockScoreService.GetBreakingResistanceScore(stock).TotalScore;
            totalScore += _stockScoreService.GetTradeScore(stock).TotalScore;

            var higherSellersScore = await _stockScoreService.GetBidAndAskScore(stock);
            totalScore += higherSellersScore.TotalScore;


            _stockScoreService.GetMa9Score(stock);
            _stockScoreService.GetMa20Score(stock);

            //2/2 winner
            //8/10 loser 

            await _stockScoreService.GetRecentlySuspendedAndBlockSaleScore(stockCode);
            await _stockScoreService.GetTrendingStockScore(stockCode);

            totalScore += await _stockScoreService.GetMostActiveAndGainerScore(stockCode);

            //Reached cap/max buying - matic 100 %
            //has a really steep down recently 20%
            await _stockScoreService.GetDividendScore(stockCode);

            _stockScoreService.GetVolumeScore(chartHistory, stock);

            //get passing score from setting
            var passingScore = 70m;
            if (totalScore >= passingScore)
                return;
        }

        public void EodStockUpdate()
        {
            var marketActivity = _investagramsApiService.GetLatestStockMarketActivity();
            UpdateBlockSale(marketActivity.Result.StockBlockSaleList);
            UpdateSuspendedStocks(marketActivity.Result.StockSuspensionList);

            //get stock list
        }

        public void UpdateInvestagramsCookie(string value)
        {
            _cookieProviderService.SetCookie(value);
        }

        public virtual DateTime GetLastTradingDate()
        {
            var lastTradingDate = _cache.Get<DateTime>(CacheNames.LastTradingDateCacheKey);
            if (lastTradingDate != DateTime.Now)
            {
                if (lastTradingDate == DateTime.MinValue)
                    lastTradingDate = DateTime.Now;
                
                if (lastTradingDate.DayOfWeek == DayOfWeek.Sunday)
                    lastTradingDate = lastTradingDate.AddDays(-2);
                if (lastTradingDate.DayOfWeek == DayOfWeek.Saturday)
                    lastTradingDate = lastTradingDate.AddDays(-1);

                //consider holiday

                _cache.Add(CacheNames.LastTradingDateCacheKey, lastTradingDate, TimeSpan.FromDays(1));
            }


            return lastTradingDate.Date;
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

