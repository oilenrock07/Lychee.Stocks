using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lychee.Stocks.InvestagramsApi.Models.Investagrams;
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
            var result = await _investagramsApiRepository.ViewStock("MWC");

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
            var result = await _investagramsApiRepository.GetAskAndBidByStockId(34);

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
    }
}
