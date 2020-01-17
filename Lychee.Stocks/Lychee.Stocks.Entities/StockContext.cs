using System.Data.Entity;

namespace Lychee.Stocks.Entities
{
    public class StockContext : DbContext
    {
        public StockContext() : base("ConnectionString.MsSql")
        {
            
        }

        public virtual IDbSet<Stock> Stocks { get; set; }
        public virtual IDbSet<StockHistory> StockHistory { get; set; }
        public virtual IDbSet<TechnicalAnalysis> TechnicalAnalysis { get; set; }

        public virtual IDbSet<MyPrediction> Predictions { get; set; }
    }
}
