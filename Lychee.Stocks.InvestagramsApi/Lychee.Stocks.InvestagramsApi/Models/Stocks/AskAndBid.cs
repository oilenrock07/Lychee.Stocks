namespace Lychee.Stocks.InvestagramsApi.Models.Stocks
{
    public class AskAndBid
    {
        public Buyer[] Buyers { get; set; }
        public Seller[] Sellers { get; set; }
    }

    public class Buyer
    {
        public int StockId { get; set; }
        public int OrderVerb { get; set; }
        public decimal Price { get; set; }
        public decimal Volume { get; set; }
        public decimal NumberOfOrders { get; set; }
    }

    public class Seller
    {
        public int StockId { get; set; }
        public int OrderVerb { get; set; }
        public decimal Price { get; set; }
        public decimal Volume { get; set; }
        public decimal NumberOfOrders { get; set; }
    }

}
