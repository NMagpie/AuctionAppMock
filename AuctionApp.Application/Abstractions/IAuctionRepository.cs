using AuctionApp.Domain.Models;

namespace AuctionApp.Application.Abstractions;
public interface IAuctionRepository
{
    Auction Create(Auction auction);

    Auction? GetById(int id);

    Auction? AddLotById(int id, Lot lot);

    ICollection<Auction> GetAll();

    int GetLastId();
}
