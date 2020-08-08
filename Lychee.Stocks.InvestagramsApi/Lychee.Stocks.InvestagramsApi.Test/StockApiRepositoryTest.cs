using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lychee.Stocks.InvestagramsApi.Models.Stocks;
using Lychee.Stocks.InvestagramsApi.Repositories;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Lychee.Stocks.InvestagramsApi.Test
{
    [TestFixture]
    public class StockApiRepositoryTest : BaseApiRepositoryTest
    {
        private StockApiRepository _investagramsApiRepository;
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _investagramsApiRepository = new StockApiRepository(_cookieProviderService);
        }

        [Test]
        public async Task CanGetLatestStockMarketActivity()
        {
            //Act
            var result = await _investagramsApiRepository.GetLatestStockMarketActivity();

            //Asserts
            Assert.That(result, Is.Not.Null);
            Assert.That(result.TotalValue, Is.GreaterThan(0));
        }

        [Test]
        public async Task CanViewStock()
        {
            //Act
            var result = await _investagramsApiRepository.ViewStock("BDO");

            //Asserts
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StockInfo, Is.Not.Null);
            Assert.That(result.StockInfo.StockCode, Is.Not.Empty);
        }

        [Test]
        public async Task CanGetAllActiveStockRealTimePrice()
        {
            //Act
            var result = await _investagramsApiRepository.GetAllActiveStockPriceRealTime(1);

            //Asserts
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.GreaterThan(1));
            Assert.That(result.First().StockCode, Is.Not.Empty);
        }

        [Test]
        public async Task CanGetAskAndBidByStockId()
        {
            //Act
            var result = await _investagramsApiRepository.GetAskAndBidByStockId(142);

            var buyers = result.Buyers.OrderByDescending(x => x.Volume);
            var sellers = result.Sellers.OrderByDescending(x => x.Volume);

            //Asserts
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Buyers.Length, Is.GreaterThan(1));
        }


        [Test]
        public async Task CanGetMarketStatus()
        {
            //Act
            var result = await _investagramsApiRepository.GetMarketStatus(DateTime.Now);

            //Asserts
            Assert.That(result, Is.Not.Null);
            Assert.That(result.MostActive.Length, Is.GreaterThan(1));
        }

        [Test]
        public async Task CanGetLatestTechnicalAnalysis()
        {
            //Act
            var result = await _investagramsApiRepository.GetLatestTechnicalAnalysis("MAC");

            //Asserts
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task CanGetLatestStockHistoryByStockId()
        {
            //Act
            var result = await _investagramsApiRepository.GetLatestStockHistoryByStockId(81);

            //Asserts
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task CanGetChartHistory()
        {
            //Act
            var result = await _investagramsApiRepository.GetChartHistoryByDate(142, DateTime.Now);

            var ave = result.Volumes.Take(15).Average();

            //Asserts
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task CanGetChartByMinute()
        {
            //Act
            var result = await _investagramsApiRepository.GetChartByMinutes("FNI", 5);

            //Asserts
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task CanGetBullBearData()
        {
            //Act
            var result = await _investagramsApiRepository.GetBullBearData(142);

            //Asserts
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task CanGetAllLatestStocks()
        {
            //Act
            var result = await _investagramsApiRepository.GetScreenerResponse(new Screener());

            //Asserts
            Assert.That(result, Is.Not.Null);
        }


        [Test]
        public async Task CanGetDisclosureNews()
        {
            //Act
            var result = await _investagramsApiRepository.GetDisclosureNews();

            //Asserts
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task CanGetBusinessNews()
        {
            //Act
            var result = await _investagramsApiRepository.GetBusinessNews();

            //Asserts
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task CanGetFinancialReportNews()
        {
            //Act
            var result = await _investagramsApiRepository.GetFinancialReportNews();

            //Asserts
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task CanGetNewsByStockId()
        {
            //Act
            var result = await _investagramsApiRepository.GetNewsByStockId(158);

            //Asserts
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task DownloadChart()
        {
            //Arrange
            var code = "MPI";
            var date = DateTime.Now;

            //Act
            var stock = await _investagramsApiRepository.ViewStockWithoutFundamentalAnalysis(code);
            var result = await _investagramsApiRepository.GetChartHistoryByDate(stock.StockInfo.StockId, date);

            var stringResult = Utf8Json.JsonSerializer.ToJsonString(result);

            var fileName = $@"C:\StocksDownloadedData\{code}-{date:yyyyMMdd}.json";
            using (var file = new System.IO.StreamWriter(fileName, true))
            {
                file.WriteLine(stringResult);
            }

            //Asserts
            Assert.That(result, Is.Not.Null);
        }


        [Test]
        public async Task TplTest()
        {
            //5.29
            var startTime = DateTime.Now;
            var technicalAnalysis = await _investagramsApiRepository.GetLatestTechnicalAnalysis("NOW");
            var latestStock = await _investagramsApiRepository.GetScreenerResponse(new Screener());
            var bullBear = await _investagramsApiRepository.GetBullBearData(142);

            //1 seconds
            var task1 = Task.Factory.StartNew(() => _investagramsApiRepository.GetLatestTechnicalAnalysis("NOW"));
            var task2 = Task.Factory.StartNew(() => _investagramsApiRepository.GetScreenerResponse(new Screener()));
            var task3 = Task.Factory.StartNew(() => _investagramsApiRepository.GetBullBearData(142));

            //3.38
            var totalScore = 0m;
            var tasks = new List<Task>();
            tasks.Add(Task.Run(async () => await _investagramsApiRepository.GetLatestTechnicalAnalysis("NOW")).ContinueWith((a) => totalScore += a.Result.VolumeAvg10));
            tasks.Add(Task.Run(async () => await _investagramsApiRepository.GetScreenerResponse(new Screener()).ContinueWith((a) => totalScore += a.Result.Count)));
            tasks.Add(Task.Run(async () => await _investagramsApiRepository.GetBullBearData(142)).ContinueWith((a) => totalScore += a.Result.BuyingAvePrice));
            Task.WaitAll(tasks.ToArray());

            var endTime = DateTime.Now.Subtract(startTime).TotalSeconds;
        }
    }
}
