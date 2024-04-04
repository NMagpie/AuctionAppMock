using AuctionApp.Application.Abstractions;
using AuctionApp.Application.Auctions.Create;
using AuctionApp.Domain.Models;
using Moq;

namespace AuctionApp.Tests.AuctionApp.Application.Tests.AuctionsService;
public class CreateAuctionTests
{

    [Fact]
    public async void CreateAuctionGood()
    {
        var auction = new CreateAuction("123", DateTime.Now.AddDays(1), TimeSpan.FromMinutes(10));

        var auctionRepositoryMock = new Mock<IAuctionRepository>();

        auctionRepositoryMock.Setup(x => x.Create(It.IsAny<Auction>())).Returns<Auction?>(r => new Auction());

        var createAuctionHandler = new CreateAuctionHandler(auctionRepositoryMock.Object);

        await createAuctionHandler.Handle(auction, new CancellationToken());

        auctionRepositoryMock.Verify(x => x.Create(It.IsAny<Auction>()), Times.Once);
    }

    [Fact]
    public void CreateAuctionWithoutTitle()
    {
        var auctionRepositoryMock = new Mock<IAuctionRepository>();

        auctionRepositoryMock.Setup(x => x.Create(It.IsAny<Auction>())).Returns<Auction?>(null);

        var createAuctionHandler = new CreateAuctionHandler(auctionRepositoryMock.Object);

        var auction = new CreateAuction("", DateTime.Now.AddMinutes(10), TimeSpan.FromMinutes(10));

        Assert.Multiple(
            () => auctionRepositoryMock.Verify(r => r.Create(It.IsAny<Auction>()), Times.Never),
            async () => await Assert.ThrowsAsync<ApplicationException>(async () => await createAuctionHandler.Handle(auction, new CancellationToken()))
            );
    }

    [Fact]
    public void CreateAuctionWithPassedTimeStart()
    {
        var auctionRepositoryMock = new Mock<IAuctionRepository>();

        auctionRepositoryMock.Setup(x => x.Create(It.IsAny<Auction>())).Returns<Auction?>(null);

        var createAuctionHandler = new CreateAuctionHandler(auctionRepositoryMock.Object);

        var auction = new CreateAuction("123", DateTime.UtcNow.Subtract(TimeSpan.FromHours(1)), TimeSpan.FromMinutes(10));

        Assert.Multiple(
            async () => await Assert.ThrowsAsync<ApplicationException>(async () => await createAuctionHandler.Handle(auction, new CancellationToken())),
            () => auctionRepositoryMock.Verify(r => r.Create(It.IsAny<Auction>()), Times.Never)
            );
    }

    [Fact]
    public void CreateAuctionWithInvalidDuration()
    {
        var auctionRepositoryMock = new Mock<IAuctionRepository>();

        auctionRepositoryMock.Setup(x => x.Create(It.IsAny<Auction>())).Returns<Auction?>(null);

        var createAuctionHandler = new CreateAuctionHandler(auctionRepositoryMock.Object);

        var auction = new CreateAuction("123", DateTime.UtcNow, TimeSpan.FromMinutes(5));

        Assert.Multiple(
            () => auctionRepositoryMock.Verify(r => r.Create(It.IsAny<Auction>()), Times.Never),
            async () => await Assert.ThrowsAsync<ApplicationException>(async () => await createAuctionHandler.Handle(auction, new CancellationToken()))
            );
    }
}
