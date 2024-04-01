using AuctionApp.Domain.Models;

namespace AuctionApp.Application.Abstractions
{
    public interface ILotRepository
    {
        Lot Create(Lot lot);

        Lot? GetById(int id);

        List<Lot> GetLotsByIds(List<int> lotIds);

        Lot? AddBidById(int id, Bid bid);

        int GetLastId();
    }
}
