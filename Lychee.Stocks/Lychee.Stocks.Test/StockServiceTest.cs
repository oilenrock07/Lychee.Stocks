using System.Collections.Generic;
using System.Threading.Tasks;
using Lychee.Caching.Interfaces;
using Lychee.Domain.Interfaces;
using Lychee.Infrastructure.Interfaces;
using Lychee.Scrapper.Domain.Interfaces;
using Lychee.Scrapper.Repository.Interfaces;
using Lychee.Stocks.Domain.Interfaces.Services;
using Lychee.Stocks.Domain.Repositories;
using Lychee.Stocks.Domain.Services;
using Lychee.Stocks.Entities;
using Moq;
using NUnit.Framework;
using Lychee.HttpClientService;
using Lychee.Stocks.Domain.Constants;
using Lychee.Stocks.Domain.Interfaces.Repositories;

namespace Lychee.Stocks.Test
{
    [TestFixture]
    public class StockServiceTest
    {
        private readonly Mock<IDatabaseFactory>  _databaseFactory = new Mock<IDatabaseFactory>();
        private readonly Mock<ISettingRepository> _settingRepository = new Mock<ISettingRepository>();
        private readonly Mock<ILoggingService> _loggingService = new Mock<ILoggingService>();
        private readonly Mock<IWebQueryService> _websQueryService = new Mock<IWebQueryService>();
        private readonly Mock<IScrappedSettingRepository> _scrappedSettingRepository = new Mock<IScrappedSettingRepository>();
        private readonly Mock<IResultCollectionService> _resultCollectionService = new Mock<IResultCollectionService>();
        private readonly Mock<IColumnDefinitionRepository> _columnDefinitionRepository = new Mock<IColumnDefinitionRepository>();
        private readonly Mock<IRepository<Stock>> _stockRepository = new Mock<IRepository<Stock>>();
        private readonly Mock<IRepository<TechnicalAnalysis>> _technicalAnalysis = new Mock<IRepository<TechnicalAnalysis>>();
        private readonly Mock<IRepository<StockHistory>> _stockHistoryRepository = new Mock<IRepository<StockHistory>>();
        private readonly Mock<IRepository<MyPrediction>> _predictionRepository = new Mock<IRepository<MyPrediction>>();
        private readonly Mock<ISuspendedStockRepository> _suspendedStockRepository = new Mock<ISuspendedStockRepository>();
        private readonly Mock<BlockSaleStockRepository> _blockSaleStockRepository = new Mock<BlockSaleStockRepository>();

        //private InvestagramsApiRepository _investagramsApiRepository;
        private IStockService _stockService;

        private Mock<ICachingFactory> _cachingFactory = new Mock<ICachingFactory>();

        //[OneTimeSetUp]
        //public void OneTimeSetup()
        //{
        //    _settingRepository.Setup(x => x.GetSettingValue<string>(SettingNames.InvestagramsCookieName))
        //        .Returns("__cfduid=de323b9ac823cdc7f269a305c2e32b2c21564121419; _uc_referrer=https%3A//www.google.com/; _uc_initial_landing_page=https%3A//www.investagrams.com/; _ga=GA1.2.689041548.1564121423; _fbp=fb.1.1564121423362.1863262359; _uc_utm_medium=InvestaPlatform; _uc_utm_campaign=InvestaPrime; _uc_utm_term=; _uc_utm_content=; _uc_utm_source=PrimeButton; __tawkuuid=e::investagrams.com::HwBjbyfzeoyTLlvUdRt3JmaFlvO27w/60zDsTwo9mQoDq9lE47f46zld/1IMlWWr::2; _fbc=fb.1.1574068802060.IwAR3CrqMPypr465sGPAkvk4vny_cecYphhjvAcLKHt_RlAJdAJ4vrziquGCw; __gads=ID=8ae7617cb96d5b41:T=1579846771:S=ALNI_MaWEVwnz5BX6FRq7GWVmImL2zRHzg; _gid=GA1.2.1133112883.1580945880; _uc_last_referrer=direct; e7bfeae28c72f9a5fae2a6ce78db24e7=7717A7A56A01A305DF86531111B88208E8F4361BE5D6B39B9EE4550B988C2B7E6D27A06ECDBFDA39A39FFD14B1AA05C25512BFBFC57EFE9B5EC0C0030501F8528BA6673B8F17DA06AC89875539C0D577699FB5E1C4437DFF4AF2C0FBC30C8FE8C6CB06BAB7CF6810A59DEB6A92649143E353F311058852E9825B1AFE4146495721C4523F32B16E2C280462DAB580EA5A7F4EDFA10D3BD4E52D7E74F4CC23FA092C9E8FB19AD05CA9B68B257B316B7F038F405A18AA1F12F3407AD39E7F02D14AB1C5CC040613472CDAA608DEA99B7D266E3E2C76120477289AC12B828D71B1ADCC550D46EA43F39BE4ADB35B5DF0C27F; _uc_current_session=true; _uc_visits=224; _gat=1");

        //    var container = new SimpleInjector.Container();
        //    container.RegisterLycheeHttpClientService("https://webapi.investagrams.com/");
        //    var httpClientService = container.GetInstance<IHttpClientService>();

        //    _investagramsApiRepository = new InvestagramsApiRepository(_settingRepository.Object, httpClientService);
        //    _stockService = new StockService(_databaseFactory.Object, _settingRepository.Object, _loggingService.Object, _websQueryService.Object, _scrappedSettingRepository.Object,
        //        _resultCollectionService.Object, _columnDefinitionRepository.Object, _stockRepository.Object, _technicalAnalysis.Object, _stockHistoryRepository.Object,
        //        _cachingFactory.Object, _investagramsApiRepository, _suspendedStockRepository.Object, _blockSaleStockRepository.Object);
        //}

        [Test]
        public async Task CanGetStocksFromInvestagramApiUsingParallelTask()
        {
            var stockCodes = new List<string> {"MWC", "JFC"};
            await _stockService.UpdateStocks(stockCodes);
        }

        //[Test]
        //public void ExistsOnAllTest()
        //{
        //    //Arrange
        //    var stockService = new StockService(_databaseFactory.Object, _settingRepository.Object,
        //        _loggingService.Object, _websQueryService.Object, _scrappedSettingRepository.Object,
        //        _resultCollectionService.Object, _columnDefinitionRepository.Object, _stockRepository.Object,
        //        _technicalAnalysis.Object, _stockHistoryRepository.Object, _predictionRepository.Object);

        //    var fiveOverFive = new List<StockTrendReportModel>
        //    {
        //        new StockTrendReportModel {StockCode = "JFC"},
        //        new StockTrendReportModel {StockCode = "ALI"}
        //    };
        //    var tenOverEight = new List<StockTrendReportModel>
        //    {
        //        new StockTrendReportModel {StockCode = "MWC"},
        //        new StockTrendReportModel {StockCode = "JFC"}
        //    };
        //    var twentyOverFifteen = new List<StockTrendReportModel>
        //    {
        //        new StockTrendReportModel {StockCode = "MWC"},
        //        new StockTrendReportModel {StockCode = "JFC"},
        //        new StockTrendReportModel {StockCode = "AXLM"}
        //    };

        //    //Act
        //    var list = stockService.ExistsOnAll(fiveOverFive, tenOverEight, twentyOverFifteen);

        //    //Asserts
        //    Assert.That(list, Is.Not.Null);
        //    Assert.That(list.Count, Is.EqualTo(1));
        //    Assert.That(list.First(), Is.EqualTo("JFC"));

        //}
    }
}
