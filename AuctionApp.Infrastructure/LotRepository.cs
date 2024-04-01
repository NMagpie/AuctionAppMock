using AuctionApp.Application.Abstractions;
using AuctionApp.Domain.Models;

namespace AuctionApp.Infrastructure
{
    public class LotRepository : ILotRepository
    {
        private readonly List<Lot> _lots = [];

        public Lot Create(Lot lot)
        {
            _lots.Add(lot);
            return lot;
        }

        public Lot? GetById(int id)
        {
            return _lots.FirstOrDefault(a => a.Id == id);
        }

        public int GetLastId()
        {
            if (_lots.Count == 0) return 1;
            var lastId = _lots.Max(b => b.Id);
            return lastId + 1;
        }

        public List<Lot> GetLotsByIds(List<int> lotIds)
        {
            var lots = _lots.Where(lot => lotIds.Contains(lot.Id)).Select(lot => lot).ToList();
            return lots;
        }
        public Lot? AddBidById(int id, Bid bid)
        {
            var lot = _lots.FirstOrDefault(a => a.Id == id);

            lot?.Bids.Add(bid);

            return lot;
        }
    }
}
