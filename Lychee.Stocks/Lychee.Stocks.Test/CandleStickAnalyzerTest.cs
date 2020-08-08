using System.IO;
using Lychee.Stocks.Domain.Services;
using Lychee.Stocks.InvestagramsApi.Models.Stocks;
using NUnit.Framework;

namespace Lychee.Stocks.Test
{
    [TestFixture]
    public class CandleStickAnalyzerTest
    {
        private CandleStickAnalyzerService _analyzer;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _analyzer = new CandleStickAnalyzerService();
        }

        [Test]
        public void EveningStarDojiTest()
        {
            //Arrange
            var file = $"{TestContext.CurrentContext.TestDirectory}/Data/CandleStickAnalyzerData/EveningStars/MPI-20200808.json";
            var json = LoadFile(file);
            var data = Utf8Json.JsonSerializer.Deserialize<ChartHistory>(json);


            _analyzer.IsEveningStarDoji(data);
        }

        private string LoadFile(string fileName)
        {
            string json;
            using (var reader = new StreamReader(fileName))
            {
                json = reader.ReadToEnd();
            }
            return json;
        }
    }
}
