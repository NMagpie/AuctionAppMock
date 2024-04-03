using AuctionApp.Application.Auctions.Responses;
using AuctionApp.Application.Bids.Responses;
using AuctionApp.Domain.Models;

namespace AuctionApp.Application.Lots.Responses;
public class LotDto
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public AuctionDto? Auction { get; set; }

    public decimal InitialPrice { get; set; }

    public decimal Price { get; set; }

    public List<BidDto> Bids { get; set; }

    public static LotDto FromLot(Lot lot, bool convertAuction = false)
    {
        return new LotDto
        {
            Id = lot.Id,
            Title = lot.Title,
            Description = lot.Description,
            Auction =  convertAuction ? AuctionDto.FromAuction(lot.Auction) : null,
            InitialPrice = lot.InitialPrice,
            Price = lot.Price,
            Bids = lot.Bids.Select(bid => BidDto.FromBid(bid)).ToList()
        };
    }

    public override string ToString()
    {
        return $"Title: {Title}, Description: {Description}, Initial Price: {InitialPrice.ToString("C")}\nCurrent Price: {Price.ToString("C")} \nBids:\n{string.Join("", Bids)}\n";
    }
}
