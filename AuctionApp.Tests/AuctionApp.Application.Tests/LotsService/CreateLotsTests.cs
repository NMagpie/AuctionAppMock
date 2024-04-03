using AuctionApp.Application.Auctions.Create;
using AuctionApp.Application.Lots.Create;
using AuctionApp.Infrastructure;

namespace AuctionApp.Tests.AuctionApp.Application.Tests.LotsService;
public class CreateLotsTests
{
    [Fact]
    public async Task CreateCorrectLot()
    {
        var auctionRepositoryMock = new AuctionRepository();

        var lotRepositoryMock = new LotRepository();

        var createAuctionHandler = new CreateAuctionHandler(auctionRepositoryMock, lotRepositoryMock);

        var createLotHandler = new CreateLotHandler(lotRepositoryMock, auctionRepositoryMock);

        var createAuction = new CreateAuction("123", DateTime.Now, TimeSpan.FromMinutes(10));

        var auctionResult = await createAuctionHandler.Handle(createAuction, new CancellationToken());

        var createLot = new CreateLot("123", "456", auctionResult.Id, 10m);

        var lotResult = await createLotHandler.Handle(createLot, new CancellationToken());

        var checkObjects = createLot.Title.Equals(lotResult.Title) &&
            createLot.Description.Equals(lotResult.Description) &&
            createLot.InitialPrice == lotResult.InitialPrice;

        Assert.True(checkObjects);
    }

    [Fact]
    public async Task CreateLotWithEmptyTitle()
    {
        var auctionRepositoryMock = new AuctionRepository();

        var lotRepositoryMock = new LotRepository();

        var createAuctionHandler = new CreateAuctionHandler(auctionRepositoryMock, lotRepositoryMock);

        var createLotHandler = new CreateLotHandler(lotRepositoryMock, auctionRepositoryMock);

        var createAuction = new CreateAuction("123", DateTime.Now, TimeSpan.FromMinutes(10));

        var auctionResult = await createAuctionHandler.Handle(createAuction, new CancellationToken());

        var createLot = new CreateLot("", "456", auctionResult.Id, 10m);

        await Assert.ThrowsAsync<ApplicationException>(async () => await createLotHandler.Handle(createLot, new CancellationToken()));
    }

    [Fact]
    public async Task CreateLotWithNullDescription()
    {
        var auctionRepositoryMock = new AuctionRepository();

        var lotRepositoryMock = new LotRepository();

        var createAuctionHandler = new CreateAuctionHandler(auctionRepositoryMock, lotRepositoryMock);

        var createLotHandler = new CreateLotHandler(lotRepositoryMock, auctionRepositoryMock);

        var createAuction = new CreateAuction("123", DateTime.Now, TimeSpan.FromMinutes(10));

        var auctionResult = await createAuctionHandler.Handle(createAuction, new CancellationToken());

        var createLot = new CreateLot("123", null, auctionResult.Id, 10m);

        await Assert.ThrowsAsync<ApplicationException>(async () => await createLotHandler.Handle(createLot, new CancellationToken()));
    }

    [Fact]
    public async Task CreateLotWithNegativePrice()
    {
        var auctionRepositoryMock = new AuctionRepository();

        var lotRepositoryMock = new LotRepository();

        var createAuctionHandler = new CreateAuctionHandler(auctionRepositoryMock, lotRepositoryMock);

        var createLotHandler = new CreateLotHandler(lotRepositoryMock, auctionRepositoryMock);

        var createAuction = new CreateAuction("123", DateTime.Now, TimeSpan.FromMinutes(10));

        var auctionResult = await createAuctionHandler.Handle(createAuction, new CancellationToken());

        var createLot = new CreateLot("123", "456", auctionResult.Id, -10m);

        await Assert.ThrowsAsync<ApplicationException>(async () => await createLotHandler.Handle(createLot, new CancellationToken()));
    }

    [Fact]
    public async Task CreateLotWithInvalidAuctionId()
    {
        var auctionRepositoryMock = new AuctionRepository();

        var lotRepositoryMock = new LotRepository();

        var createAuctionHandler = new CreateAuctionHandler(auctionRepositoryMock, lotRepositoryMock);

        var createLotHandler = new CreateLotHandler(lotRepositoryMock, auctionRepositoryMock);

        var createAuction = new CreateAuction("123", DateTime.Now, TimeSpan.FromMinutes(10));

        var auctionResult = await createAuctionHandler.Handle(createAuction, new CancellationToken());

        var createLot = new CreateLot("123", "456", -1, 10m);

        await Assert.ThrowsAsync<ApplicationException>(async () => await createLotHandler.Handle(createLot, new CancellationToken()));
    }
}

