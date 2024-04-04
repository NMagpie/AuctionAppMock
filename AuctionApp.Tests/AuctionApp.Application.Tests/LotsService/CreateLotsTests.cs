using AuctionApp.Application.Abstractions;
using AuctionApp.Application.Lots.Create;
using AuctionApp.Domain.Models;
using Moq;

namespace AuctionApp.Tests.AuctionApp.Application.Tests.LotsService;
public class CreateLotsTests
{
    [Fact]
    public async Task CreateCorrectLot()
    {
        var auctionRepositoryMock = new Mock<IAuctionRepository>();

        auctionRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Auction());

        var lotRepositoryMock = new Mock<ILotRepository>();

        lotRepositoryMock.Setup(x => x.Create(It.IsAny<Lot>())).Returns(new Lot());

        var createLotHandler = new CreateLotHandler(lotRepositoryMock.Object, auctionRepositoryMock.Object);

        var createLot = new CreateLot("123", "345", 1, 11m);

        await createLotHandler.Handle(createLot, new CancellationToken());

        Assert.Multiple(
            () => auctionRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once),
            () => lotRepositoryMock.Verify(x => x.Create(It.IsAny<Lot>()), Times.Once)
            );
    }

    [Fact]
    public void CreateLotWithEmptyTitle()
    {
        var auctionRepositoryMock = new Mock<IAuctionRepository>();

        auctionRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns<Auction>(null);

        var lotRepositoryMock = new Mock<ILotRepository>();

        lotRepositoryMock.Setup(x => x.Create(It.IsAny<Lot>())).Returns<Lot>(null);

        var createLotHandler = new CreateLotHandler(lotRepositoryMock.Object, auctionRepositoryMock.Object);

        var createLot = new CreateLot("", "345", 1, 11m);

        Assert.Multiple(
            () => Assert.ThrowsAsync<ApplicationException>(async () => await createLotHandler.Handle(createLot, new CancellationToken())),
            () => auctionRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Never),
            () => lotRepositoryMock.Verify(x => x.Create(It.IsAny<Lot>()), Times.Never)
            );
    }

    [Fact]
    public void CreateLotWithNullDescription()
    {
        var auctionRepositoryMock = new Mock<IAuctionRepository>();

        auctionRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns<Auction>(null);

        var lotRepositoryMock = new Mock<ILotRepository>();

        lotRepositoryMock.Setup(x => x.Create(It.IsAny<Lot>())).Returns<Lot>(null);

        var createLotHandler = new CreateLotHandler(lotRepositoryMock.Object, auctionRepositoryMock.Object);

        var createLot = new CreateLot("123", null, 1, 11m);

        Assert.Multiple(
            () => Assert.ThrowsAsync<ApplicationException>(async () => await createLotHandler.Handle(createLot, new CancellationToken())),
            () => auctionRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Never),
            () => lotRepositoryMock.Verify(x => x.Create(It.IsAny<Lot>()), Times.Never)
            );
    }

    [Fact]
    public void CreateLotWithNegativePrice()
    {
        var auctionRepositoryMock = new Mock<IAuctionRepository>();

        auctionRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns<Auction>(null);

        var lotRepositoryMock = new Mock<ILotRepository>();

        lotRepositoryMock.Setup(x => x.Create(It.IsAny<Lot>())).Returns<Lot>(null);

        var createLotHandler = new CreateLotHandler(lotRepositoryMock.Object, auctionRepositoryMock.Object);

        var createLot = new CreateLot("123", "345", 1, -11m);

        Assert.Multiple(
            () => Assert.ThrowsAsync<ApplicationException>(async () => await createLotHandler.Handle(createLot, new CancellationToken())),
            () => auctionRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Never),
            () => lotRepositoryMock.Verify(x => x.Create(It.IsAny<Lot>()), Times.Never)
            );
    }

    [Fact]
    public void CreateLotWithInvalidAuctionId()
    {
        var auctionRepositoryMock = new Mock<IAuctionRepository>();

        auctionRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns<Auction>(null);

        var lotRepositoryMock = new Mock<ILotRepository>();

        lotRepositoryMock.Setup(x => x.Create(It.IsAny<Lot>())).Returns<Lot>(null);

        var createLotHandler = new CreateLotHandler(lotRepositoryMock.Object, auctionRepositoryMock.Object);

        var createLot = new CreateLot("123", "345", -1, 11m);

        Assert.Multiple(
            () => Assert.ThrowsAsync<ApplicationException>(async () => await createLotHandler.Handle(createLot, new CancellationToken())),
            () => auctionRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once),
            () => lotRepositoryMock.Verify(x => x.Create(It.IsAny<Lot>()), Times.Never)
            );
    }
}

