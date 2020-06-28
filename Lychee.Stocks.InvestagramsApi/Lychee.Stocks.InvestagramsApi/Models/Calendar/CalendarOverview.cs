using System;

namespace Lychee.Stocks.InvestagramsApi.Models.Calendar
{
    public class CalendarOverview
    {
        public object[] Seminars { get; set; }
        public Dividend[] Dividends { get; set; }
        public Others[] Others { get; set; }
    }

    public class Dividend
    {
        public int Id { get; set; }
        public int StockId { get; set; }
        public string StockCode { get; set; }
        public string StockName { get; set; }
        public int DividendType { get; set; }
        public string DividendTypeString { get; set; }
        public decimal DividendValue { get; set; }
        public string DividendValueString { get; set; }
        public DateTime ExDate { get; set; }
        public DateTime RecordDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public string SourceUrl { get; set; }
    }

    public class Others
    {
        public int CalendarId { get; set; }
        public string Region { get; set; }
        public int CalendarType { get; set; }
        public string CalendarTypeString { get; set; }
        public DateTime BegDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
    }

}
