using AuctionApp.Domain.Enumerators;

namespace AuctionApp.Domain.Models;
public class Auction
{
    private readonly List<Lot> _lots = [];

    private readonly List<Bid> _bids = [];

    public Auction()
    {
        Lots = _lots;
        Bids = _bids;
    }

    public int Id { get; set; }

    public string Title { get; set; }

    public DateTime TimeStart { get; set; }

    public TimeSpan Duration { get; set; }

    public AuctionStatus Status { get; set; }

    public ICollection<Lot> Lots { get; set; }

    public ICollection<Bid> Bids { get; set; }
}
