namespace AuctionApp.Domain.Models
{
    public class Lot
    {
        private readonly List<Bid> _bids = [];

        public Lot()
        {
            Bids = _bids;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal InitialPrice { get; set; }
        public decimal Price { get; set; }

        public ICollection<Bid> Bids { get; set; }
    }
}
