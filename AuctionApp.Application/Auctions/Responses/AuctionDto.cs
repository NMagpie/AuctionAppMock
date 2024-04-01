using AuctionApp.Application.Bids.Responses;
using AuctionApp.Application.Lots.Responses;
using AuctionApp.Domain.Enumerators;
using AuctionApp.Domain.Models;

namespace AuctionApp.Application.Auctions.Responses
{
    public class AuctionDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime TimeStart { get; set; }

        public TimeSpan Duration { get; set; }

        public AuctionStatus Status { get; set; }

        public List<LotDto> Lots { get; set; }

        public List<BidDto> Bids { get; set; }

        public static AuctionDto FromAuction(Auction auction)
        {
            return new AuctionDto
            {
                Id = auction.Id,
                Title = auction.Title,
                TimeStart = auction.TimeStart,
                Duration = auction.Duration,
                Status = auction.Status,
                Lots = auction.Lots.Select(LotDto.FromLot).ToList(),
                Bids = auction.Bids.Select(bid => BidDto.FromBid(bid) ).ToList()
            };
        }

        public override string ToString()
        {
            return $"Id: {Id}\nTitle: {Title}\nStart time: {TimeStart}\nDuration: {Duration.TotalMinutes} min\nStatus: {Status}\nLots:\n {string.Join("", Lots)}\n";
        }
    }
}
