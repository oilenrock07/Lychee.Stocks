using System;

namespace Lychee.Stocks.InvestagramsApi.Models.Stocks
{
    public class TechnicalAnalysisInfo
    {
        public int StockId { get; set; }
        public decimal Support1 { get; set; }
        public decimal Support2 { get; set; }
        public decimal Resistance1 { get; set; }
        public decimal Resistance2 { get; set; }
        public int TrendShort { get; set; }
        public int TrendMedium { get; set; }
        public int TrendLong { get; set; }
        public int Recommendation { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public DateTime Date { get; set; }
        public decimal Ma9 { get; set; }
        public decimal Ma20 { get; set; }
        public int Ma20Trend { get; set; }
        public decimal Ma50 { get; set; }
        public int Ma50Trend { get; set; }
        public decimal Ma100 { get; set; }
        public int Ma100Trend { get; set; }
        public decimal Ma200 { get; set; }
        public int Ma200Trend { get; set; }
        public decimal Ema9 { get; set; }
        public decimal Ema12 { get; set; }
        public decimal Ema13 { get; set; }
        public decimal Ema20 { get; set; }
        public int Ema20Trend { get; set; }
        public decimal Ema26 { get; set; }
        public decimal Ema50 { get; set; }
        public int Ema50Trend { get; set; }
        public decimal Ema100 { get; set; }
        public int Ema100Trend { get; set; }
        public decimal Ema200 { get; set; }
        public int Ema200Trend { get; set; }
        public decimal MacdLine { get; set; }
        public decimal MacdSignalLine { get; set; }
        public decimal MacdHistogram { get; set; }
        public int MacdStockMarketTrend { get; set; }
        public decimal High52 { get; set; }
        public decimal Low52 { get; set; }
        public decimal Rsi14 { get; set; }
        public int Rsi14Trend { get; set; }
        public string Rsi14AdditionalTrend { get; set; }
        public decimal Atr { get; set; }
        public decimal AtrVolatilityPercentage { get; set; }
        public string AtrVolatilityGauge { get; set; }
        public decimal Cci { get; set; }
        public int CciStockMarketTrend { get; set; }
        public decimal VolumeAvg10 { get; set; }
        public int VolumeAvg10Trend { get; set; }
        public decimal VolumeAvg15 { get; set; }
        public int VolumeAvg15Trend { get; set; }
        public decimal VolumeAvg20 { get; set; }
        public int VolumeAvg20Trend { get; set; }
        public decimal ValueAvg10 { get; set; }
        public decimal ValueAvg15 { get; set; }
        public decimal ValueAvg20 { get; set; }
        public int TradeAvg10 { get; set; }
        public int TradeAvg15 { get; set; }
        public int TradeAvg20 { get; set; }
        public decimal YearToDate { get; set; }
        public decimal MonthToDate { get; set; }
        public decimal WeekToDate { get; set; }
        public decimal NetForeign5Days { get; set; }
        public decimal NetForeign15Days { get; set; }
        public decimal NetForeign30Days { get; set; }
        public decimal NetForeign60Days { get; set; }
        public decimal NetForeign360Days { get; set; }
        public decimal StsFastK { get; set; }
        public decimal StsSlowK { get; set; }
        public decimal StsSlowD { get; set; }
        public int StsSlowMarketTrend { get; set; }
        public decimal WilliamsR { get; set; }
        public int WilliamsRStockMarketTrend { get; set; }
        public int SingleCandleStickPattern { get; set; }
        public int SingleCandleStickPatternTrend { get; set; }
        public int DoubleCandleStickPattern { get; set; }
        public int DoubleCandleStickPatternTrend { get; set; }
        public int TripleCandleStickPattern { get; set; }
        public int TripleCandleStickPatternTrend { get; set; }
        public decimal BollingerBandUpper { get; set; }
        public decimal BollingerBandMiddle { get; set; }
        public decimal BollingerBandLower { get; set; }
        public decimal BollingerBandWidth { get; set; }
    }

}
