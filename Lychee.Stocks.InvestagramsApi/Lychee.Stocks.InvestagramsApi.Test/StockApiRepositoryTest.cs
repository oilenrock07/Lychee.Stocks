using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var result = await _investagramsApiRepository.ViewStock("GMA7");

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
            var result = await _investagramsApiRepository.GetLatestTechnicalAnalysis("NOW");

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

            var ave = result.Volumes.Take(20).Average();

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
    }
}
