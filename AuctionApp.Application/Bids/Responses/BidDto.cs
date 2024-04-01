using AuctionApp.Application.Auctions.Responses;
using AuctionApp.Application.Lots.Responses;
using AuctionApp.Domain.Models;

namespace AuctionApp.Application.Bids.Responses;
public class BidDto
{
    public int Id { get; set; }

    public AuctionDto? Auction { get; set; }

    public LotDto? Lot { get; set; }

    public decimal Price { get; set; }

    public DateTime Time { get; set; }

    public static BidDto FromBid(Bid bid, bool convertAuction = false, bool convertLot = false)
    {
        return new BidDto
        {
            Id = bid.Id,
            Auction = convertAuction ? AuctionDto.FromAuction(bid.Auction) : null,
            Lot = convertLot ? LotDto.FromLot(bid.Lot) : null,
            Price = bid.Price,
            Time = bid.Time,
        };
    }

    public override string ToString()
    {
        return $"Id: {Id}, Price: {Price.ToString("C")}, Time: {Time}\n";
    }
}
