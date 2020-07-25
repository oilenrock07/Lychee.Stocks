﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lychee.Stocks.InvestagramsApi.Models.Calendar;
using Lychee.Stocks.InvestagramsApi.Models.Social;
using Lychee.Stocks.InvestagramsApi.Models.Stocks;

namespace Lychee.Stocks.InvestagramsApi.Interfaces
{
    public interface IInvestagramsApiService 
    {
        Task<LatestStockMarketActivityVm> GetLatestStockMarketActivity();

        Task<ViewStock> ViewStock(string stockCode);

        Task<ViewStock> ViewStockWithoutFundamentalAnalysis(string stockCode);

        Task<List<RealTimePrice>> GetAllActiveStockPriceRealTime();

        Task<AskAndBid> GetAskAndBidByStockId(int investagramStockId);

        Task<MarketStatus> GetMarketStatus(DateTime date);

        Task<LatestStockHistory> GetLatestStockHistoryByStockId(int stockId);
        Task<TechnicalAnalysisInfo> GetLatestTechnicalAnalysis(string stockCode);

        Task<List<TrendingStock>> GetTrendingStocks();

        Task<CalendarOverview> GetCalendarOverview();

        Task<ChartHistory> GetChartHistoryByDate(int stockId, DateTime date);

        Task<BullBearData> GetBullBearData(int stockId);

        Task<List<ScreenerResponse>> GetScreenerResponse(Screener screener);

        Task<List<News>> GetDisclosureNews(int lastStockNewsId = -1);


        Task<List<News>> GetFinancialReportNews(int lastStockNewsId = -1);


        Task<List<News>> GetBusinessNews(int lastStockNewsId = -1);

        Task<List<News>> GetNewsByStockId(int stockId, int lastStockNewsId = -1);

        Task<ChartByMinute> GetChartByMinutes(string stockCode, int minute);
    }
}
