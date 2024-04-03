using AuctionApp.Domain.Enumerators;

namespace AuctionApp.Domain.Models;
public class Auction
{
    public int Id { get; set; }

    public string Title { get; set; }

    public DateTime TimeStart { get; set; }

    public TimeSpan Duration { get; set; }

    public AuctionStatus Status { get; set; }

    public ICollection<Lot> Lots { get; set; } = [];

    public ICollection<Bid> Bids { get; set; } = [];
}
