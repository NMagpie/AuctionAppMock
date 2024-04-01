using AuctionApp.Application.Bids.Responses;
using AuctionApp.Domain.Models;

namespace AuctionApp.Application.Lots.Responses;
public class LotDto
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public decimal InitialPrice { get; set; }

    public List<BidDto> Bids { get; set; }

    public static LotDto FromLot(Lot lot)
    {
        return new LotDto
        {
            Id = lot.Id,
            Title = lot.Title,
            Description = lot.Description,
            InitialPrice = lot.InitialPrice,
            Bids = lot.Bids.Select(BidDto.FromBid).ToList()
        };
    }
}
