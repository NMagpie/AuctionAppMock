using AuctionApp.Application.Abstractions;
using AuctionApp.Application.Auctions.Create;
using AuctionApp.Application.Auctions.Responses;
using AuctionApp.Application.Bids.Responses;
using AuctionApp.Application.Lots.Create;
using AuctionApp.Application.Lots.Responses;
using AuctionApp.Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AucitonApp.Tests;

public class UnitTest1
{

    private readonly ServiceProvider _serviceProvider;

    private readonly IMediator _mediator;

    //[Theory]
    //[MemberData(nameof(TestDataCreateLot))]
    public async void TestCreateLot(CreateAuction request, AuctionDto? expectedResult)
    {
        var result = await _mediator.Send(request);

        Assert.Equal(expectedResult, result);
    }

    public static TheoryData<CreateAuction, AuctionDto?> TestDataCreateLot => new()
    {

    };

    //[Theory]
    //[MemberData(nameof(TestDataCreateAuction))]
    public async void TestCreateAuction(CreateAuction request, AuctionDto? expectedResult)
    {
        var result = await _mediator.Send(request);

        Assert.Equal(expectedResult, result);
    }

    public static TheoryData<CreateAuction, AuctionDto?> TestDataCreateAuction => new()
    {

    };

    public UnitTest1()
    {
        _serviceProvider = new ServiceCollection()
            .AddSingleton<IAuctionRepository, AuctionRepository>()
            .AddSingleton<ILotRepository, LotRepository>()
            .AddSingleton<IBidRepository, BidRepository>()
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IAuctionRepository).Assembly))
            .BuildServiceProvider();

        _mediator = _serviceProvider.GetRequiredService<IMediator>();
    }

    [Fact]
    public async void CreateAuctionWithoutLots()
    {
        var auction = await _mediator.Send(new CreateAuction("Famous Worldwide paintings", DateTime.UtcNow, TimeSpan.FromMinutes(30), []));

        Assert.IsType<AuctionDto>(auction);

        Assert.True(auction.Lots.Count == 0);
    }

    [Fact]
    public async void CreateLotWithEmptyTitleAndDescription()
    {
        async Task<LotDto> sendRequest() => await _mediator.Send(new CreateLot("", "", 2_760_000m));

        await Assert.ThrowsAsync<ApplicationException>(sendRequest);
    }

    [Fact]
    public async void CreateBidWithNonExistingIds()
    {
        async Task<BidDto> sendRequest() => await _mediator.Send(new CreateBid(-1, -1, 10m));

        await Assert.ThrowsAsync<ApplicationException>(sendRequest);
    }

    [Fact]
    public async void CreateBidWithNegativePrice()
    {
        async Task<BidDto> sendRequest() => await _mediator.Send(new CreateBid(1, 1, -10m));

        await Assert.ThrowsAsync<ApplicationException>(sendRequest);
    }
}