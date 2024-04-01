namespace AuctionApp.Domain.Models;
public class Bid
{
    public Bid()
    {

    }

    public int Id { get; set; }

    public Auction Auction { get; set; }

    public Lot Lot { get; set; }

    public decimal Price { get; set; }

    public DateTime Time { get; set; }
}