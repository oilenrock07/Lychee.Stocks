using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using Lychee.Domain.Interfaces;
using Lychee.Scrapper.Domain.Helpers;
using Lychee.Stocks.Domain.Constants;
using Lychee.Stocks.Domain.Enums;
using Lychee.Stocks.Domain.Interfaces.Services;
using Lychee.Stocks.Domain.Models.Investagrams;
using Lychee.Stocks.InvestagramsApi.Interfaces;
using Lychee.Stocks.InvestagramsApi.Models.Stocks;
using ViewStock = Lychee.Stocks.InvestagramsApi.Models.Stocks.ViewStock;

namespace Lychee.Stocks.Domain.Services
{
    public class StockScoreService : IStockScoreService
    {
        private readonly IInvestagramsApiService _investagramsApiService;
        private readonly ISettingRepository _settingRepository;

        public StockScoreService(IInvestagramsApiService investagramsApiService, ISettingRepository settingRepository)
        {
            _investagramsApiService = investagramsApiService;
            _settingRepository = settingRepository;
        }



        /// <summary>
        /// If stock is top gainer and most active then get the perfect score, otherwise get half if either of the two is true
        /// </summary>
        /// <param name="stockCode"></param>
        /// <returns></returns>
        public async Task<decimal> GetMostActiveAndGainerScore(string stockCode)
        {
            var perfectScore = _settingRepository.GetSettingValue<decimal>(SettingNames.Score_MostActiveAndTopGainer);

            var marketStatus = await _investagramsApiService.GetMarketStatus(DateTime.Now);
            var isInMostActive = marketStatus.MostActive.Any(x => x.StockCode == stockCode);
            var isInTopGainer = marketStatus.TopGainer.Any(x => x.StockCode == stockCode);

            if (isInTopGainer && isInMostActive)
                return perfectScore;

            if (isInMostActive)
                return perfectScore / 2;

            if (isInTopGainer)
                return perfectScore / 2;

            return 0;
        }

        public async Task<StockScore> GetTrendingStockScore(string stockCode)
        {
            var score = new StockScore();
            var perfectScore = _settingRepository.GetSettingValue<decimal>(SettingNames.Score_Trending);
            
            var trendingStocks =  await _investagramsApiService.GetTrendingStocks();

            //maybe add how frequent this stock is already on the market. if more than 3 days then give less score
            if (trendingStocks.Any(x => x.Stock.StockCode == stockCode))
            {
                score.AddBreakdown(perfectScore, "Trending stock");
            }

            return score;
        }

        public async Task<StockScore> GetDividendScore(string stockCode)
        {
            var score = new StockScore();
            var perfectScore = _settingRepository.GetSettingValue<decimal>(SettingNames.Score_Trending);

            var calendar = await _investagramsApiService.GetCalendarOverview();
            var dividend = calendar.Dividends.FirstOrDefault(x => x.StockCode == stockCode);
            if (dividend != null)
            {
                score.AddBreakdown(perfectScore, $"Will be giving dividend soon Ex Date: {dividend.ExDate} Payment Date: {dividend.PaymentDate}");
            }

            return score;
        }

        public StockScore GetMa9Score(ViewStock viewStock)
        {
            var score = new StockScore();
            var perfectScore = _settingRepository.GetSettingValue<decimal>(SettingNames.Score_Ma9);

            if (viewStock.LatestStockHistory.Last > viewStock.StockTechnicalAnalysisInfo.Ma9)
                score.AddBreakdown(perfectScore, "Bullish MA 9");

            return score;
        }

        public StockScore GetMa20Score(ViewStock viewStock)
        {
            var score = new StockScore();
            var perfectScore = _settingRepository.GetSettingValue<decimal>(SettingNames.Score_Ma20);

            if (viewStock.LatestStockHistory.Last > viewStock.StockTechnicalAnalysisInfo.Ma20)
                score.AddBreakdown(perfectScore, "Bullish MA 20");

            return score;
        }

        public StockScore GetVolumeScore(ChartHistory chartHistory, ViewStock viewStock)
        {
            var score = new StockScore();
            var perfectScore = _settingRepository.GetSettingValue<decimal>(SettingNames.Score_Volume);

            var averageVolume20 = chartHistory.Volumes.Take(20).Average().ToDecimal();
            if (viewStock.LatestStockHistory.Volume > averageVolume20)
                score.AddBreakdown(perfectScore, "Has volume");

            return score;
        }

        public StockScore GetBreakingResistanceScore(ViewStock viewStock)
        {
            var score = new StockScore();
            var perfectScore = _settingRepository.GetSettingValue<decimal>(SettingNames.Score_MostActiveAndTopGainer);

            //break resistance 1
            if (viewStock.LatestStockHistory.Last >= viewStock.StockTechnicalAnalysisInfo.Resistance1)
                score.AddBreakdown(perfectScore * 0.65m, "Broken resistance 1");

            //break resistance 2
            if (viewStock.LatestStockHistory.Last >= viewStock.StockTechnicalAnalysisInfo.Resistance2)
                score.AddBreakdown(perfectScore * 0.35m, "Broken resistance 2");

            return score;
        }

        public StockScore GetBreakingSupport2Score(ViewStock viewStock)
        {
            var score = new StockScore();
            var perfectScore = _settingRepository.GetSettingValue<decimal>(SettingNames.Score_BreakSupport2);

            //break resistance 2
            if (viewStock.LatestStockHistory.Last >= viewStock.StockTechnicalAnalysisInfo.Resistance2)
                score.AddBreakdown(perfectScore, "Broken support 2");

            return score;
        }

        public StockScore GetTradeScore(ViewStock viewStock)
        {
            var score = new StockScore();
            var perfectScore = _settingRepository.GetSettingValue<decimal>(SettingNames.Score_ReachedCap);

            var tradeThreshold = 1500;
            if (viewStock.LatestStockHistory.Trades > tradeThreshold)
                score.AddBreakdown(perfectScore, $"Stock reached more than {tradeThreshold} trades");

            return score;
        }

        public async Task<StockScore> GetRecentlySuspendedAndBlockSaleScore(string stockCode)
        {
            var score = new StockScore();
            var recentlySuspendedScore = _settingRepository.GetSettingValue<decimal>(SettingNames.Score_RecentlySuspended);
            var blockSaleScore = _settingRepository.GetSettingValue<decimal>(SettingNames.Score_BlockSale);

            var marketActivity = await _investagramsApiService.GetLatestStockMarketActivity();
            if (marketActivity.StockSuspensionList.Any(x => x.StockCode == stockCode))
            {
                score.AddBreakdown(recentlySuspendedScore, "Recently added to suspended list");
            }

            if (marketActivity.StockBlockSaleList.Any(x => x.StockCode == stockCode))
            {
                score.AddBreakdown(blockSaleScore, "In block sale");
            }

            return score;
        }


        public StockScore GetReachedCapScore(ViewStock viewStock)
        {
            var score = new StockScore();
            var perfectScore = _settingRepository.GetSettingValue<decimal>(SettingNames.Score_ReachedCap);

            var cap = viewStock.LatestStockHistory.MarketCap; //find out how to find the market cap
            var lastPrice = viewStock.LatestStockHistory.Last;


            return score;
            //if (cap == lastPrice)
        }

        /// <summary>
        /// Calculate if stock is within ideal range of RSI
        /// Give negative score if stock is oversold
        /// </summary>
        /// <param name="viewStock"></param>
        /// <returns></returns>
        public StockScore GetRsiScore(ViewStock viewStock)
        {
            var score = new StockScore();
            var perfectScore = _settingRepository.GetSettingValue<decimal>(SettingNames.Score_OverSold);

            var rsi = viewStock.StockTechnicalAnalysisInfo.Rsi14;
            if (rsi > 75)
                score.AddBreakdown(perfectScore * -1, "Already overbought"); //give negative score
            else if (rsi >= 60 && rsi <= 70)
                score.AddBreakdown(perfectScore, $"RSI is {rsi}");
            else if (rsi <= 30 && rsi > 20)
                score.AddBreakdown(perfectScore * 0.3m, "Watch this stock"); //give 30% of perfect score
            if (rsi < 10)
                score.AddBreakdown(perfectScore, "Oversold");

            return score;
        }



        public async Task<StockScore> GetBidAndAskScore(ViewStock stock)
        {
            var score = new StockScore();
            var perfectScore = _settingRepository.GetSettingValue<decimal>(SettingNames.Score_AskAndBid);

            var askAndBids = await _investagramsApiService.GetAskAndBidByStockId(stock.StockInfo.StockId);
            var buyers = askAndBids.Buyers.OrderByDescending(x => x.Volume).ToList();
            var sellers = askAndBids.Sellers.OrderByDescending(x => x.Volume).ToList();


            //Nobody wants to sell because there is a breakout.
            //There must be some good news with this stock or this is being hyped
            if (buyers.Count > 0 && sellers.Count == 0)
            {
                score.ImportantConfirmation = true;
                score.Trend = StockTrend.Bullish;
            }
            else if (buyers.Count == 0 && sellers.Count > 0)
            {
                //ideally this shouldn't happen
                score.ImportantConfirmation = true;
                score.Trend = StockTrend.Bearish;
            }
            else
            {
                var highestVolumeOfBuyers = buyers.First();
                var highestVolumeOfSellers = sellers.First();

                //My Own metrics.
                //Inverse law of supply and demand
                //if sellers is greater than the buyers then it must be going up. (must be backed up with volume)
                if (highestVolumeOfSellers.Volume > highestVolumeOfBuyers.Volume)
                {
                    var percentage = (highestVolumeOfSellers.Volume - highestVolumeOfBuyers.Volume) / highestVolumeOfSellers.Volume;
                    var reason = $"Sellers are {percentage * 100}% higher than the buyers";

                    //if sellers are 15-30% higher than buyers give half score
                    if (percentage >= 0.15m && percentage <= 0.3m)
                    {
                        score.AddBreakdown(perfectScore / 2, reason);
                    }
                    //if sellers are 50-30% higher than the buyers give the perfect score
                    else if (percentage >= 0.3m)
                    {
                        score.AddBreakdown(perfectScore, reason);
                    }
                }
            }

            return score;
        }
    }
}
