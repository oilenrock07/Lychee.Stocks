using System.Threading.Tasks;
using Lychee.Stocks.InvestagramsApi.Interfaces;
using Lychee.Stocks.InvestagramsApi.Repositories;
using NUnit.Framework;

namespace Lychee.Stocks.InvestagramsApi.Test
{
    [TestFixture]
    public class CalendarApiRepositoryTest : BaseApiRepositoryTest
    {
        private ICalendarApiRepository _investagramsApiRepository;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _investagramsApiRepository = new CalendarApiRepository();
            _investagramsApiRepository.AddCookies = () => _investagramsApiRepository.ParseCookie(GetCookie());
        }

        [Test]
        public async Task CanGetCalendarOverview()
        {
            //Act
            var result = await _investagramsApiRepository.GetCalendarOverview();

            //Asserts
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Dividends, Is.Not.Null);
        }
    }
}
