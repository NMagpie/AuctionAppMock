using AuctionApp.Application.Auctions.Responses;
using AuctionApp.Application.Lots.Responses;
using AuctionApp.Domain.Models;

namespace AuctionApp.Application.Bids.Responses
{
    public class BidDto
    {
        public int Id { get; set; }

        public AuctionDto Auction { get; set; }

        public LotDto Lot { get; set; }

        public decimal Price { get; set; }

        public DateTime Time { get; set; }

        public static BidDto FromBid(Bid bid)
        {
            return new BidDto
            {
                Id = bid.Id,
                Auction = AuctionDto.FromAuction(bid.Auction),
                Lot = LotDto.FromLot(bid.Lot),
                Price = bid.Price,
                Time = bid.Time,
            };
        }
    }
}
