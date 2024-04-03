using AuctionApp.Application.Auctions.Create;
using AuctionApp.Application.Auctions.Responses;
using AuctionApp.Infrastructure;
using AutoFixture;
using Castle.DynamicProxy.Generators;
using System.Reflection;

namespace AuctionApp.Tests.AuctionApp.Application.Tests.AuctionsService;
public class CreateAuctionTests
{
    [Fact]
    public async void CreateCorrectAuction()
    {
        var auctionRepositoryMock = new AuctionRepository();

        var lotRepositoryMock = new LotRepository();

        var createAuctionHandler = new CreateAuctionHandler(auctionRepositoryMock, lotRepositoryMock);

        var auction = new CreateAuction("123", DateTime.Now.AddDays(1), TimeSpan.FromMinutes(10));

        var auctionResult = await createAuctionHandler.Handle(auction, new CancellationToken());

        Assert.Multiple(
            () => Assert.IsType<AuctionDto>(auctionResult),
            () => Assert.True(auctionResult.Lots.Count == 0)
            );
    }

    [Fact]
    public async Task CreateAuctionWithoutTitle()
    {

        var auctionRepositoryMock = new AuctionRepository();

        var lotRepositoryMock = new LotRepository();

        var createAuctionHandler = new CreateAuctionHandler(auctionRepositoryMock, lotRepositoryMock);

        var auction = new CreateAuction("", DateTime.Now.AddMinutes(10), TimeSpan.FromMinutes(10));

        await Assert.ThrowsAsync<ApplicationException>(async () => await createAuctionHandler.Handle(auction, new CancellationToken()));
    }

    [Fact]
    public async Task CreateAuctionWithPassedTimeStart()
    {
        var auctionRepositoryMock = new AuctionRepository();

        var lotRepositoryMock = new LotRepository();

        var createAuctionHandler = new CreateAuctionHandler(auctionRepositoryMock, lotRepositoryMock);

        var auction = new CreateAuction("", DateTime.Now.Subtract(TimeSpan.FromHours(1)), TimeSpan.FromMinutes(10));

        await Assert.ThrowsAsync<ApplicationException>(async () => await createAuctionHandler.Handle(auction, new CancellationToken()));
    }

    [Fact]
    public async Task CreateAuctionWithInvalidDuration()
    {
        var auctionRepositoryMock = new AuctionRepository();

        var lotRepositoryMock = new LotRepository();

        var createAuctionHandler = new CreateAuctionHandler(auctionRepositoryMock, lotRepositoryMock);

        var auction = new CreateAuction("123", DateTime.UtcNow, TimeSpan.FromMinutes(5));

        await Assert.ThrowsAsync<ApplicationException>(async () => await createAuctionHandler.Handle(auction, new CancellationToken()));
    }
}
