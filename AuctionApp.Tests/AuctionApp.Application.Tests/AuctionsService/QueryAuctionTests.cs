using AuctionApp.Application.Auctions.Create;
using AuctionApp.Application.Auctions.Queries;
using AuctionApp.Infrastructure;
using AutoFixture;

namespace AuctionApp.Tests.AuctionApp.Application.Tests.AuctionsService;
public class QueryAuctionTests
{
    [Fact]
    public async Task GetAuctionById()
    {
        var auctionRepositoryMock = new AuctionRepository();

        var lotRepositoryMock = new LotRepository();

        var createAuctionHandler = new CreateAuctionHandler(auctionRepositoryMock, lotRepositoryMock);

        var getAuctionByIdHandler = new GetAuctionByIdHandler(auctionRepositoryMock);

        var createAuction = new CreateAuction("123", DateTime.Now.AddMinutes(10), TimeSpan.FromMinutes(10));

        var auctionResult = await createAuctionHandler.Handle(createAuction, new CancellationToken());

        var getAuction = new GetAuctionById(auctionResult.Id);

        var queryResult = await getAuctionByIdHandler.Handle(getAuction, new CancellationToken());

        Assert.Equivalent(auctionResult, queryResult);
    }

    [Fact]
    public async Task GetAllAuctions()
    {
        var fixture = new Fixture();

        fixture.Customizations.Add(new RandomDateTimeSequenceGenerator(DateTime.Now, DateTime.MaxValue));

        fixture.Customizations.Add(new TimeSpanGenerator());

        var auctionRepositoryMock = new AuctionRepository();

        var lotRepositoryMock = new LotRepository();

        var createAuctionHandler = new CreateAuctionHandler(auctionRepositoryMock, lotRepositoryMock);

        var getAllAuctionsHandler = new GetAllAuctionsHandler(auctionRepositoryMock);

        var createAuctionList = fixture.Create<Generator<CreateAuction>>()
            .Take(100)
            .ToList();

        var auctionResultList = createAuctionList
            .Select(async (createAuction) => await createAuctionHandler.Handle(createAuction, new CancellationToken()))
            .Select(auction => auction.Result).ToList();

        var getAuctionQuery = new GetAllAuctions();

        var queryResult = (await getAllAuctionsHandler.Handle(getAuctionQuery, new CancellationToken())).ToList();

        Assert.Equivalent(auctionResultList, queryResult);
    }
}
