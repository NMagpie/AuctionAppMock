using AuctionApp.Application.Abstractions;
using AuctionApp.Domain.Models;

namespace AuctionApp.Infrastructure
{
    public class BidRepository : IBidRepository
    {
        private readonly List<Bid> _bids = [];

        public Bid Create(Bid bid)
        {
            _bids.Add(bid);
            return bid;
        }

        public Bid? GetById(int id)
        {
            var bid = _bids.FirstOrDefault(b => b.Id == id);
            return bid;
        }

        public int GetLastId()
        {
            if (_bids.Count == 0) return 1;
            var lastId = _bids.Max(b => b.Id);
            return lastId + 1;
        }
    }
}
