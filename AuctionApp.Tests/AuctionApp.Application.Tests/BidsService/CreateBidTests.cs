using AuctionApp.Application.Abstractions;
using AuctionApp.Application.Lots.Create;
using AuctionApp.Domain.Models;
using Moq;

namespace AuctionApp.Tests.AuctionApp.Application.Tests.BidsService;
public class CreateBidTests
{
    [Fact]
    public async Task CreateCorrectBid()
    {
        var auctionRepositoryMock = new Mock<IAuctionRepository>();

        auctionRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Auction());

        var lotRepositoryMock = new Mock<ILotRepository>();

        lotRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Lot());

        var bidRepositoryMock = new Mock<IBidRepository>();

        bidRepositoryMock.Setup(x => x.Create(It.IsAny<Bid>())).Returns(new Bid());

        var createBidHandler = new CreateBidHandler(bidRepositoryMock.Object, lotRepositoryMock.Object, auctionRepositoryMock.Object);

        var createBid = new CreateBid(1, 1, 11m);

        await createBidHandler.Handle(createBid, new CancellationToken());

        Assert.Multiple(
            () => auctionRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once),
            () => lotRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once),
            () => bidRepositoryMock.Verify(x => x.Create(It.IsAny<Bid>()), Times.Once)
            );
    }

    [Fact]
    public void CreateBidWithLowerPrice()
    {
        var auctionRepositoryMock = new Mock<IAuctionRepository>();

        auctionRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Auction());

        var lotRepositoryMock = new Mock<ILotRepository>();

        lotRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Lot() { Price = 11m });

        var bidRepositoryMock = new Mock<IBidRepository>();

        bidRepositoryMock.Setup(x => x.Create(It.IsAny<Bid>())).Returns<Bid>(null);

        var createBidHandler = new CreateBidHandler(bidRepositoryMock.Object, lotRepositoryMock.Object, auctionRepositoryMock.Object);

        var createBid = new CreateBid(1, 1, 10m);

        Assert.Multiple(
            () => Assert.ThrowsAsync<ApplicationException>(async () => await createBidHandler.Handle(createBid, new CancellationToken())),
            () => auctionRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once),
            () => lotRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once),
            () => bidRepositoryMock.Verify(x => x.Create(It.IsAny<Bid>()), Times.Never)
            );
    }

    [Fact]
    public void CreateBidWithInvalidAuctionId()
    {
        var auctionRepositoryMock = new Mock<IAuctionRepository>();

        auctionRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns<Auction>(x => null);

        var lotRepositoryMock = new Mock<ILotRepository>();

        lotRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns<Lot>(x => null);

        var bidRepositoryMock = new Mock<IBidRepository>();

        bidRepositoryMock.Setup(x => x.Create(It.IsAny<Bid>())).Returns<Bid>(x => null);

        var createBidHandler = new CreateBidHandler(bidRepositoryMock.Object, lotRepositoryMock.Object, auctionRepositoryMock.Object);

        var createBid = new CreateBid(-1, 1, 10m);

        Assert.Multiple(
            () => Assert.ThrowsAsync<ApplicationException>(async () => await createBidHandler.Handle(createBid, new CancellationToken())),
            () => auctionRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once),
            () => lotRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Never),
            () => bidRepositoryMock.Verify(x => x.Create(It.IsAny<Bid>()), Times.Never)
            );
    }

    [Fact]
    public void CreateBidWithInvalidLotId()
    {
        var auctionRepositoryMock = new Mock<IAuctionRepository>();

        auctionRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Auction());

        var lotRepositoryMock = new Mock<ILotRepository>();

        lotRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns<Lot>(x => null);

        var bidRepositoryMock = new Mock<IBidRepository>();

        bidRepositoryMock.Setup(x => x.Create(It.IsAny<Bid>())).Returns<Bid>(x => null);

        var createBidHandler = new CreateBidHandler(bidRepositoryMock.Object, lotRepositoryMock.Object, auctionRepositoryMock.Object);

        var createBid = new CreateBid(1, -1, 10m);

        Assert.Multiple(
            () => Assert.ThrowsAsync<ApplicationException>(async () => await createBidHandler.Handle(createBid, new CancellationToken())),
            () => auctionRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once),
            () => lotRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once),
            () => bidRepositoryMock.Verify(x => x.Create(It.IsAny<Bid>()), Times.Never)
            );
    }
}
