namespace AuctionApp.Domain.Models;
public class Lot
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public Auction Auction { get; set; }

    public decimal InitialPrice { get; set; }
    public decimal Price { get; set; }

    public ICollection<Bid> Bids { get; set; } = [];
}
