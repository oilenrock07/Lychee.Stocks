using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lychee.Infrastructure.Interfaces;
using Lychee.Scrapper.Domain.Extensions;
using Lychee.Scrapper.Domain.Helpers;
using Lychee.Scrapper.Domain.Interfaces;
using Lychee.Scrapper.Domain.Models.Scrappers;
using Lychee.Scrapper.Entities.Entities;
using Lychee.Scrapper.Repository.Interfaces;
using Lychee.Stocks.Domain.Helpers;
using Lychee.Stocks.Domain.Interfaces.Services;
using Lychee.Stocks.Entities;
using Newtonsoft.Json.Linq;
using PuppeteerSharp;

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
        private readonly Infrastructure.Interfaces.IRepository<Stock> _stockRepository;
        private readonly Infrastructure.Interfaces.IRepository<TechnicalAnalysis> _technicalAnalysis;
        private readonly Infrastructure.Interfaces.IRepository<StockHistory> _stockHistoryRepository;
        private readonly Infrastructure.Interfaces.IRepository<MyPrediction> _predictionRepository;

        public StockService(IDatabaseFactory databaseFactory, ISettingRepository settingRepository, 
            ILoggingService loggingService, IWebQueryService websQueryService,
            IScrappedSettingRepository scrappedSettingRepository, IResultCollectionService resultCollectionService,
            IColumnDefinitionRepository columnDefinitionRepository,
            Infrastructure.Interfaces.IRepository<Stock> stockRepository,
            Infrastructure.Interfaces.IRepository<TechnicalAnalysis> technicalAnalysis,
            Infrastructure.Interfaces.IRepository<StockHistory> stockHistoryRepository, Infrastructure.Interfaces.IRepository<MyPrediction> predictionRepository)
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
            _predictionRepository = predictionRepository;
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
                catch (Exception ex)
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
                        SOpen = item.GetItem("Open").Value.ToDecimal(),
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
                    stock.SOpen = item.GetItem("Open").Value.ToDecimal();
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

        public ICollection<MyPrediction> GetLast5DaysPredictions()
        {
            var date = DateTime.Now.AddDays(-5);
            return _predictionRepository
                .Find(x => x.DateCreated >= date)
                .OrderByDescending(x => x.DateCreated)
                .ToList();
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

        private async Task ScrapeRealTimeMonitoringData(Page page, ResultCollection<ResultItemCollection> resultCollection, Dictionary<string, object> args)
        {
            var resultSelector = "#StockQuoteTable";
            await page.WaitForSelectorAsync(resultSelector);

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
                        SOpen = item.Value[(value.Key, "Open")].ToDecimal(),
                        Volume = item.Value[(value.Key, nameof(StockHistory.Volume))].ConvertToNumber()
                    };
                    _stockHistoryRepository.Add(stockHistory);
                }
            }

            _stockHistoryRepository.SaveChanges();
        }

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
    }
}
