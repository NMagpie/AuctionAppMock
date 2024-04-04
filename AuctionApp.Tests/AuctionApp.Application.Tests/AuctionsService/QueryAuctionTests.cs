using AuctionApp.Application.Abstractions;
using AuctionApp.Application.Auctions.Queries;
using AuctionApp.Domain.Models;
using Moq;

namespace AuctionApp.Tests.AuctionApp.Application.Tests.AuctionsService;
public class QueryAuctionTests
{
    [Fact]
    public async Task GetAuctionById()
    {
        var auctionRepositoryMock = new Mock<IAuctionRepository>();

        auctionRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Auction());

        var getAuctionByIdHandler = new GetAuctionByIdHandler(auctionRepositoryMock.Object);

        await getAuctionByIdHandler.Handle(new GetAuctionById(1), new CancellationToken());

        auctionRepositoryMock.Verify(r => r.GetById(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAuctions()
    {
        var auctionRepositoryMock = new Mock<IAuctionRepository>();

        auctionRepositoryMock.Setup(x => x.GetAll()).Returns([]);

        var getAllAuctionsHandler = new GetAllAuctionsHandler(auctionRepositoryMock.Object);

        await getAllAuctionsHandler.Handle(new GetAllAuctions(), new CancellationToken());

        auctionRepositoryMock.Verify(r => r.GetAll(), Times.Once);
    }
}
