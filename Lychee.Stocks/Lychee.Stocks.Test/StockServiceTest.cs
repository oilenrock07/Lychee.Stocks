using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lychee.Infrastructure.Interfaces;
using Lychee.Scrapper.Domain.Interfaces;
using Lychee.Scrapper.Repository.Interfaces;
using Lychee.Stocks.Domain.Services;
using Lychee.Stocks.Entities;
using Moq;
using NUnit.Framework;

namespace Lychee.Stocks.Test
{
    [TestFixture]
    public class StockServiceTest
    {
        private Mock<IDatabaseFactory>  _databaseFactory;
        private Mock<ISettingRepository> _settingRepository;
        private Mock<ILoggingService> _loggingService;
        private Mock<IWebQueryService> _websQueryService;
        private Mock<IScrappedSettingRepository> _scrappedSettingRepository;
        private Mock<IResultCollectionService> _resultCollectionService;
        private Mock<IColumnDefinitionRepository> _columnDefinitionRepository;
        private Mock<Infrastructure.Interfaces.IRepository<Stock>> _stockRepository;
        private Mock<Infrastructure.Interfaces.IRepository<TechnicalAnalysis>> _technicalAnalysis;
        private Mock<Infrastructure.Interfaces.IRepository<StockHistory>> _stockHistoryRepository;
        private Mock<Infrastructure.Interfaces.IRepository<MyPrediction>> _predictionRepository;
        

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _databaseFactory = new Mock<IDatabaseFactory>();
            _settingRepository = new Mock<ISettingRepository>();
            _loggingService = new Mock<ILoggingService>();
            _websQueryService = new Mock<IWebQueryService>();
            _scrappedSettingRepository = new Mock<IScrappedSettingRepository>();
            _resultCollectionService = new Mock<IResultCollectionService>();
            _columnDefinitionRepository = new Mock<IColumnDefinitionRepository>();
            _stockRepository = new Mock<Infrastructure.Interfaces.IRepository<Stock>>();
            _technicalAnalysis = new Mock<Infrastructure.Interfaces.IRepository<TechnicalAnalysis>>();
            _stockHistoryRepository = new Mock<Infrastructure.Interfaces.IRepository<StockHistory>>();
            _predictionRepository = new Mock<Infrastructure.Interfaces.IRepository<MyPrediction>>();
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
