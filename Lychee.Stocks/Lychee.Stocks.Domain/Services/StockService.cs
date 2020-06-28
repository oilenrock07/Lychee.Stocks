using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using LazyCache;
using Lychee.Caching.Interfaces;
using Lychee.Domain.Interfaces;
using Lychee.Infrastructure.Interfaces;
using Lychee.Scrapper.Domain.Extensions;
using Lychee.Scrapper.Domain.Helpers;
using Lychee.Scrapper.Domain.Interfaces;
using Lychee.Scrapper.Domain.Models.Scrappers;
using Lychee.Scrapper.Entities.Entities;
using Lychee.Scrapper.Repository.Interfaces;
using Lychee.Stocks.Domain.Constants;
using Lychee.Stocks.Domain.Helpers;
using Lychee.Stocks.Domain.Interfaces.Repositories;
using Lychee.Stocks.Domain.Interfaces.Services;
using Lychee.Stocks.Domain.Models.Investagrams;
using Lychee.Stocks.Entities;
using Newtonsoft.Json.Linq;
using PuppeteerSharp;
using SuspendedStock = Lychee.Stocks.Domain.Models.Investagrams.SuspendedStock;

namespace Lychee.Stocks.Domain.Services
{
    public class StockService : IStockService
    {
        private readonly IDatabaseFactory _databaseFactory;
        private readonly ISettingRepository _settingRepository;
        private readonly ILoggingService _loggingService;
        private readonly IWebQueryService _websQueryService;
        private readonly IScrappedSettingRepository _scrappedSettingRepository;
        private readonly IResultCollectionService _resultCollectionService;
        private readonly IColumnDefinitionRepository _columnDefinitionRepository;
        private readonly IRepository<Stock> _stockRepository;
        private readonly IRepository<TechnicalAnalysis> _technicalAnalysis;
        private readonly IRepository<StockHistory> _stockHistoryRepository;
        private readonly IAppCache _cache;
        private readonly ISuspendedStockRepository _suspendedStockRepository;
        private readonly IBlockSaleStockRepository _blockSaleStockRepository;

        private readonly string _investaCookie = "cache:investa-cookie";

        public StockService(IDatabaseFactory databaseFactory, ISettingRepository settingRepository, 
            ILoggingService loggingService, IWebQueryService websQueryService,
            IScrappedSettingRepository scrappedSettingRepository, IResultCollectionService resultCollectionService,
            IColumnDefinitionRepository columnDefinitionRepository,
            IRepository<Stock> stockRepository,
            IRepository<TechnicalAnalysis> technicalAnalysis,
            IRepository<StockHistory> stockHistoryRepository, ICachingFactory cacheFactory, ISuspendedStockRepository suspendedStockRepository, IBlockSaleStockRepository blockSaleStockRepository)
        {
            _databaseFactory = databaseFactory;
            _settingRepository = settingRepository;
            _loggingService = loggingService;
            _websQueryService = websQueryService;
            _scrappedSettingRepository = scrappedSettingRepository;
            _resultCollectionService = resultCollectionService;
            _columnDefinitionRepository = columnDefinitionRepository;
            _stockRepository = stockRepository;
            _technicalAnalysis = technicalAnalysis;
            _stockHistoryRepository = stockHistoryRepository;
            _suspendedStockRepository = suspendedStockRepository;
            _blockSaleStockRepository = blockSaleStockRepository;
            _cache = cacheFactory.GetCacheService();
        }

        public async Task UpdateAllStocks()
        {
            //get stock list
            var stocks = _stockRepository.GetAll().ToList();

            var scrapper = new SmartScrapper(_settingRepository, _loggingService, _websQueryService)
            {
                IsHeadless = true,
                CustomScrappingInstructions = new List<SmartScrapper.CustomScrapping>
                {
                    ScrapeTechnicalAnalysisData,
                    ScrapeHistoricalData
                }
            };

            foreach (var stock in stocks)
            {
                scrapper.Url = $"https://www.investagrams.com/Stock/PSE:{stock.StockCode}";
                scrapper.Parameters = new Dictionary<string, object>
                {
                    {"StockCode", stock.StockCode}
                };

                try
                {
                    var scrappedItems = await scrapper.Scrape();
                    var mappedData = _resultCollectionService.MapToScrappedData(scrappedItems);

                    SaveTechnicalAnalysis(mappedData);
                    SaveStockHistory(mappedData);
                }
                catch (System.Exception ex)
                {
                    _loggingService.Logger.Error(ex, "");
                }
            }
        }

        public async Task<ResultCollection<ResultItemCollection>> FetchRealTimeStocks()
        {
            var scrapper = new SmartScrapper(_settingRepository, _loggingService, _websQueryService)
            {
                IsHeadless = true,
                Url = "https://www.investagrams.com/Stock/RealTimeMonitoring",
                CustomScrappingInstructions = new List<SmartScrapper.CustomScrapping>
                {
                    LogIn,
                    ScrapeRealTimeMonitoringData
                }
            };

            return await scrapper.Scrape();
        }

        public async Task<ResultCollection<ResultItemCollection>> UpdateLogInCookie()
        {
            _cache.Remove(_investaCookie);
            var scrapper = new SmartScrapper(_settingRepository, _loggingService, _websQueryService)
            {
                IsHeadless = true,
                Url = "https://www.investagrams.com/Login/",
                CustomScrappingInstructions = new List<SmartScrapper.CustomScrapping>
                {
                    LogIn,
                    SetLoggedInCookie
                }
            };

            return await scrapper.Scrape();
        }

        public void SaveStocks(ResultCollection<ResultItemCollection> collections)
        {
            var date = DateTime.Now.Date;
            foreach (var item in collections)
            {
                var stock = _stockHistoryRepository.FirstOrDefault(x => x.StockCode == item.Key && x.Date == date);
                if (stock == null)
                {
                    _stockHistoryRepository.Add(new StockHistory
                    {
                        Date = DateTime.Now,
                        StockCode = item.Key,
                        Change = item.GetItem("Change").Value.ToDecimal(),
                        ChangePercentage =  item.GetItem("ChangePerc").Value.ToString().TrimEnd('%').ToDecimal(),
                        High = item.GetItem("High").Value.ToDecimal(),
                        Last = item.GetItem("Last").Value.ToDecimal(),
                        Low = item.GetItem("Low").Value.ToDecimal(),
                        Open = item.GetItem("Open").Value.ToDecimal(),
                        Volume = item.GetItem("Volume").Value.ToString().ConvertToNumber()
                    });
                }
                else
                {
                    stock.Change = item.GetItem("Change").Value.ToDecimal();
                    stock.ChangePercentage = item.GetItem("ChangePerc").Value.ToString().TrimEnd('%').ToDecimal();
                    stock.High = item.GetItem("High").Value.ToDecimal();
                    stock.Last = item.GetItem("Last").Value.ToDecimal();
                    stock.Low = item.GetItem("Low").Value.ToDecimal();
                    stock.Open = item.GetItem("Open").Value.ToDecimal();
                    stock.Volume = item.GetItem("Volume").Value.ToString().ConvertToNumber();
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


        public virtual async Task<ICollection<SuspendedStock>> GetSuspendedStocks()
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
            var suspendedStocks = await GetSuspendedStocks();
            if (!suspendedStocks.Any())
                return;

            var lastSuspensionDate = _suspendedStockRepository.GetLastStockSuspensionDate();

            if (lastSuspensionDate != suspendedStocks.First().Date)
                _suspendedStockRepository.SaveSuspendedStocks(suspendedStocks);
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
            var blockSaleStocks = await GetBlockSaleStocks();
            if (!blockSaleStocks.Any())
                return;

            var lastBlockSaleDate = _blockSaleStockRepository.GetLastBlockSaleStocksDate();

            if (lastBlockSaleDate != blockSaleStocks.First().Date)
                _blockSaleStockRepository.SaveBlockSaleStocks(blockSaleStocks);
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

        private void SaveTechnicalAnalysis(List<ScrappedData> data)
        {
            var columnDefinitions = _columnDefinitionRepository.GetAllScrappedDataColumnDefinitions();
            var dictionary = ScrappedDataHelper.MapToDictionary<TechnicalAnalysis>(data, columnDefinitions);

            foreach (var item in dictionary)
            {
                var technicalAnalysis = new TechnicalAnalysis
                {
                    StockCode = item.Key,
                    DateCreated = DateTime.Now,
                    Date = DateTime.Now,
                    MA100 = item.Value[nameof(TechnicalAnalysis.MA100)].ToDecimal(),
                    MA20 = item.Value[nameof(TechnicalAnalysis.MA20)].ToDecimal(),
                    MA200 = item.Value[nameof(TechnicalAnalysis.MA200)].ToDecimal(),
                    MA50 = item.Value[nameof(TechnicalAnalysis.MA50)].ToDecimal(),
                    RSI14 = item.Value[nameof(TechnicalAnalysis.RSI14)].ToDecimal(),
                    Resistance1 = item.Value[nameof(TechnicalAnalysis.Resistance1)].ToDecimal(),
                    Resistance2 = item.Value[nameof(TechnicalAnalysis.Resistance2)].ToDecimal(),
                    Support1 = item.Value[nameof(TechnicalAnalysis.Support1)].ToDecimal(),
                    Support2 = item.Value[nameof(TechnicalAnalysis.Support2)].ToDecimal()
                };
                _technicalAnalysis.Add(technicalAnalysis);
            }

            _technicalAnalysis.SaveChanges();
        }

        private void SaveStockHistory(List<ScrappedData> data)
        {
            var columnDefinitions = _columnDefinitionRepository.GetAllRelatedDataColumnDefinitions();
            var dictionary = RelatedDataHelper.MapToDictionary<StockHistory>(data, columnDefinitions, "Date");

            foreach (var item in dictionary)
            {
                foreach (var value in item.Value.GroupBy(x => x.Key.Item1))
                {
                    var stockHistory = new StockHistory
                    {
                        StockCode = item.Key,
                        Date = item.Value[(value.Key, nameof(StockHistory.Date))].ToDateTime(),
                        Last = item.Value[(value.Key, nameof(StockHistory.Last))].ToDecimal(),
                        Change = item.Value[(value.Key, nameof(StockHistory.Change))].ToDecimal(),
                        ChangePercentage = item.Value[(value.Key, "ChangePerc")].ToDecimal(),
                        High = item.Value[(value.Key, nameof(StockHistory.High))].ToDecimal(),
                        Low = item.Value[(value.Key, nameof(StockHistory.Low))].ToDecimal(),
                        Open = item.Value[(value.Key, "Open")].ToDecimal(),
                        Volume = item.Value[(value.Key, nameof(StockHistory.Volume))].ConvertToNumber()
                    };
                    _stockHistoryRepository.Add(stockHistory);
                }
            }

            _stockHistoryRepository.SaveChanges();
        }


        #region Scrapping Instructions
        private async Task ScrapeHistoricalData(Page page, ResultCollection<ResultItemCollection> resultCollection, Dictionary<string, object> args = null)
        {
            var resultSelector = "#HistoricalDataTable";
            await page.WaitForSelectorAsync(resultSelector);

            //try to get the table headers
            var mappings = _scrappedSettingRepository.GetItemSettings("HistoricalData");

            var dataSelector = "#HistoricalDataTable > tbody > tr";
            var tData = await PuppeteerHelper.GetTableData(page, dataSelector, mappings);

            var identifier = args["StockCode"].ToString();
            var collection = resultCollection.GetItem(identifier);

            var date = "";
            foreach (var item in tData)
            {
                if (date != item["Date"]?.GetValue())
                    date = item["Date"].GetValue();

                collection.Items.AddRange(mappings.Select(x => new ResultItem
                {
                    Name = x.Key,
                    Value = item[x.Key].GetValue(),
                    Group = date,
                    IsMultiple = true
                }).ToList());
            }
        }

        private async Task ScrapeTechnicalAnalysisData(Page page, ResultCollection<ResultItemCollection> resultCollection, Dictionary<string, object> args = null)
        {
            var resultSelector = "#TechnicalAnalysisContent";
            await page.WaitForSelectorAsync(resultSelector);

            //try to get the table headers
            var mappings = _scrappedSettingRepository.GetItemSettings("TechnicalAnalysisData");
            var tData = await PuppeteerHelper.GetElements(page, mappings);
            var identifier = args["StockCode"].ToString();

            resultCollection.Add(new ResultItemCollection
            {
                Key = identifier,
                Items = mappings.Select(x => new ResultItem
                {
                    Name = x.Key,
                    Value = tData.FirstOrDefault(j => j[x.Key] != null)?[x.Key].GetValue()
                }).ToList()
            });
        }

        private async Task LogIn(Page page, ResultCollection<ResultItemCollection> resultCollection, Dictionary<string, object> args)
        {
            var resultSelector = ".invest-login__input-holder";
            await page.WaitForSelectorAsync(resultSelector);

            await page.TypeAsync(".invest-login__input-holder form input[data-ng-model='LoginRequest.Username']", "cawicaancornelio@gmail.com");
            await page.TypeAsync("input[type='password']", "1234567890a!");
            await page.ClickAsync("button[data-ng-click='authenticateUser()']");
            await page.WaitForNavigationAsync();
        }

        private async Task<string> SetLoggedInCookie(Page page, ResultCollection<ResultItemCollection> resultCollection, Dictionary<string, object> args)
        {
            Task<CookieParam[]> CacheableAsyncFunc() => page.GetCookiesAsync();

            var cookies = await _cache.GetOrAddAsync(_investaCookie, CacheableAsyncFunc, TimeSpan.FromDays(3));
            return string.Join("; ", cookies.Select(x => $"{x.Name}={x.Value}"));
        }

        private async Task ScrapeRealTimeMonitoringData(Page page, ResultCollection<ResultItemCollection> resultCollection, Dictionary<string, object> args)
        {
            var resultSelector = "#StockQuoteTable";
            await page.WaitForSelectorAsync(resultSelector);

            var cookies = await page.GetCookiesAsync(page.Url);

            //try to get the table headers
            var mappings = _scrappedSettingRepository.GetItemSettings("RealTimeMonitoring");

            var dataSelector = "#StockQuoteTable > tbody > tr";
            var tData = await PuppeteerHelper.GetTableData(page, dataSelector, mappings);

            foreach (JToken link in tData)
            {
                resultCollection.Add(new ResultItemCollection
                {
                    Key = link["StockCode"].GetValue(),
                    Items = mappings.Select(x => new ResultItem
                    {
                        Name = x.Key,
                        Value = link[x.Key].GetValue()
                    }).ToList()
                });
            }
        }

        #endregion

    }
}
