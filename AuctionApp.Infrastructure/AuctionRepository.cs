using AuctionApp.Application.Abstractions;
using AuctionApp.Domain.Models;

namespace AuctionApp.Infrastructure
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly List<Auction> _auctions = [];

        public Auction Create(Auction auction)
        {
            _auctions.Add(auction);
            return auction;
        }

        public ICollection<Auction> GetAll()
        {
            return _auctions;
        }

        public Auction? GetById(int id)
        {
            return _auctions.FirstOrDefault(a => a.Id == id);
        }

        public int GetLastId()
        {
            if (_auctions.Count == 0) return 1;
            var lastId = _auctions.Max(b => b.Id);
            return lastId + 1;
        }
    }
}
