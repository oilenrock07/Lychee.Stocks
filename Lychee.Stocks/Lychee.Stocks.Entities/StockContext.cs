using System.Data.Entity;
using Lychee.Entities;

namespace Lychee.Stocks.Entities
{
    public class StockContext : LycheeContext
    {
        public StockContext() : base("ConnectionString.MsSql")
        {
            
        }

        public virtual IDbSet<Stock> Stocks { get; set; }
        public virtual IDbSet<StockHistory> StockHistory { get; set; }
        public virtual IDbSet<TechnicalAnalysis> TechnicalAnalysis { get; set; }

        public virtual IDbSet<MyPrediction> Predictions { get; set; }

        public virtual IDbSet<SuspendedStock> SuspendedStocks { get; set; }
        public virtual IDbSet<BlockSaleStock> BlockSaleStocks { get; set; }

        public virtual IDbSet<Watchlist> WatchLists { get; set; }

        public virtual IDbSet<WatchListGroup> WatchListGroups { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StockHistory>().Property(x => x.Last).HasPrecision(18, 5);
            modelBuilder.Entity<StockHistory>().Property(x => x.Open).HasPrecision(18, 5);
            modelBuilder.Entity<StockHistory>().Property(x => x.High).HasPrecision(18, 5);
            modelBuilder.Entity<StockHistory>().Property(x => x.Low).HasPrecision(18, 5);
        }
    }
}
