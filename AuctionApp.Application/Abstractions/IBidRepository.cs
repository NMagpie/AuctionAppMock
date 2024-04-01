using AuctionApp.Domain.Models;

namespace AuctionApp.Application.Abstractions;
public interface IBidRepository
{
    Bid Create(Bid bid);

    Bid? GetById(int id);

    int GetLastId();
}