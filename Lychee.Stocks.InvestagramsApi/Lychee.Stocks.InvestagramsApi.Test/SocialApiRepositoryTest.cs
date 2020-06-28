using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lychee.Stocks.InvestagramsApi.Interfaces;
using Lychee.Stocks.InvestagramsApi.Repositories;
using Lychee.Stocks.InvestagramsApi.Services;
using NUnit.Framework;

namespace Lychee.Stocks.InvestagramsApi.Test
{
    [TestFixture]
    public class SocialApiRepositoryTest : BaseApiRepositoryTest
    {
        private ISocialApiRepository _investagramsApiRepository;
        

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _investagramsApiRepository = new SocialApiRepository(_cookieProviderService);
        }

        [Test]
        public async Task CanGetTrendingStocks()
        {
            //Act
            var result = await _investagramsApiRepository.GetTrendingStocks();

            //Asserts
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.GreaterThan(0));
        }
    }
}
