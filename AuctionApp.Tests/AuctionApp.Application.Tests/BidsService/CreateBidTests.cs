using AuctionApp.Application.Auctions.Create;
using AuctionApp.Application.Lots.Create;
using AuctionApp.Infrastructure;

namespace AuctionApp.Tests.AuctionApp.Application.Tests.BidsService;
public class CreateBidTests
{
    [Fact]
    public async Task CreateCorrectBid()
    {
        var auctionRepositoryMock = new AuctionRepository();

        var lotRepositoryMock = new LotRepository();

        var bidRepositoryMock = new BidRepository();

        var createAuctionHandler = new CreateAuctionHandler(auctionRepositoryMock, lotRepositoryMock);

        var createLotHandler = new CreateLotHandler(lotRepositoryMock, auctionRepositoryMock);

        var createBidHandler = new CreateBidHandler(bidRepositoryMock, lotRepositoryMock, auctionRepositoryMock);

        var createAuction = new CreateAuction("123", DateTime.Now, TimeSpan.FromMinutes(10));

        var auctionResult = await createAuctionHandler.Handle(createAuction, new CancellationToken());

        var createLot = new CreateLot("123", "456", auctionResult.Id, 10m);

        var lotResult = await createLotHandler.Handle(createLot, new CancellationToken());

        var createBid = new CreateBid(auctionResult.Id, lotResult.Id, 11m);

        var bidResult = await createBidHandler.Handle(createBid, new CancellationToken());

        var checkObjects = createBid.Price == bidResult.Price;

        Assert.True(checkObjects);
    }

    [Fact]
    public async Task CreateBidWithLowerPrice()
    {
        var auctionRepositoryMock = new AuctionRepository();

        var lotRepositoryMock = new LotRepository();

        var bidRepositoryMock = new BidRepository();

        var createAuctionHandler = new CreateAuctionHandler(auctionRepositoryMock, lotRepositoryMock);

        var createLotHandler = new CreateLotHandler(lotRepositoryMock, auctionRepositoryMock);

        var createBidHandler = new CreateBidHandler(bidRepositoryMock, lotRepositoryMock, auctionRepositoryMock);

        var createAuction = new CreateAuction("123", DateTime.Now, TimeSpan.FromMinutes(10));

        var auctionResult = await createAuctionHandler.Handle(createAuction, new CancellationToken());

        var createLot = new CreateLot("123", "456", auctionResult.Id, 10m);

        var lotResult = await createLotHandler.Handle(createLot, new CancellationToken());

        var createBid = new CreateBid(auctionResult.Id, lotResult.Id, 9m);

        await Assert.ThrowsAsync<ApplicationException>(async () => await createBidHandler.Handle(createBid, new CancellationToken()));
    }

    [Fact]
    public async Task CreateBidWithInvalidAuctionId()
    {
        var auctionRepositoryMock = new AuctionRepository();

        var lotRepositoryMock = new LotRepository();

        var bidRepositoryMock = new BidRepository();

        var createAuctionHandler = new CreateAuctionHandler(auctionRepositoryMock, lotRepositoryMock);

        var createLotHandler = new CreateLotHandler(lotRepositoryMock, auctionRepositoryMock);

        var createBidHandler = new CreateBidHandler(bidRepositoryMock, lotRepositoryMock, auctionRepositoryMock);

        var createAuction = new CreateAuction("123", DateTime.Now, TimeSpan.FromMinutes(10));

        var auctionResult = await createAuctionHandler.Handle(createAuction, new CancellationToken());

        var createLot = new CreateLot("123", "456", auctionResult.Id, 10m);

        var lotResult = await createLotHandler.Handle(createLot, new CancellationToken());

        var createBid = new CreateBid(-1, lotResult.Id, 11m);

        await Assert.ThrowsAsync<ApplicationException>(async () => await createBidHandler.Handle(createBid, new CancellationToken()));
    }

    [Fact]
    public async Task CreateBidWithInvalidLotId()
    {
        var auctionRepositoryMock = new AuctionRepository();

        var lotRepositoryMock = new LotRepository();

        var bidRepositoryMock = new BidRepository();

        var createAuctionHandler = new CreateAuctionHandler(auctionRepositoryMock, lotRepositoryMock);

        var createLotHandler = new CreateLotHandler(lotRepositoryMock, auctionRepositoryMock);

        var createBidHandler = new CreateBidHandler(bidRepositoryMock, lotRepositoryMock, auctionRepositoryMock);

        var createAuction = new CreateAuction("123", DateTime.Now, TimeSpan.FromMinutes(10));

        var auctionResult = await createAuctionHandler.Handle(createAuction, new CancellationToken());

        var createLot = new CreateLot("123", "456", auctionResult.Id, 10m);

        var lotResult = await createLotHandler.Handle(createLot, new CancellationToken());

        var createBid = new CreateBid(auctionResult.Id, -1, 11m);

        await Assert.ThrowsAsync<ApplicationException>(async () => await createBidHandler.Handle(createBid, new CancellationToken()));
    }
}
