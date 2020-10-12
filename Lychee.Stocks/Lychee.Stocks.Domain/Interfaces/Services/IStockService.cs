using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lychee.Stocks.Domain.Models;
using Lychee.Stocks.Entities;
using Lychee.Stocks.InvestagramsApi.Models.Calendar;
using Lychee.Stocks.InvestagramsApi.Models.Stocks;

namespace Lychee.Stocks.Domain.Interfaces.Services
{
    public interface IStockService
    {
        void SaveLatestStockUpdate();

        DateTime GetLastDataUpdates();

        ICollection<StockTrendReportModel> GetStockTrendReport(int days, int losingWinningStreak,
            string trend = "Bearish");


        void UpdateInvestagramsCookie(string value);
        
        Task<StockScore> GetStockTotalScore(string stockCode);

        Task<StockAnalysisModel> AnalyzeStock(string stockCode);

        Task<List<StockAnalysisModel>> AnalyzeTrendingStock();

        Task<List<Dividend>> GetStocksGivingDividends();

        Task UpdateAllStock();

        List<StockHistory> GetStockWithSteepDown();

        List<StockTradeAverage> GetStockTradeAverages(int averageDays, int averageTrades);

        List<StockHistory> GetMorningStarDoji();

        List<StockHistory> GetHammers();

        Dictionary<string, StockHistory> GetLatestStockHistory();

        List<StockHistory> GetTop10HighestTrades(DateTime? date = null);

        List<StockHistory> GetTop10HighestVolumes(DateTime? date = null);
    }
}
