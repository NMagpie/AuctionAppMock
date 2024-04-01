using AuctionApp.Application.Abstractions;
using AuctionApp.Application.Auctions.Create;
using AuctionApp.Application.Auctions.Queries;
using AuctionApp.Application.Lots.Create;
using AuctionApp.Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

var diContainer = new ServiceCollection()
    .AddSingleton<IAuctionRepository, AuctionRepository>()
    .AddSingleton<ILotRepository, LotRepository>()
    .AddSingleton<IBidRepository, BidRepository>()
    .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IAuctionRepository).Assembly))
    .BuildServiceProvider();

var mediator = diContainer.GetRequiredService<IMediator>();

List<CreateLot> lotsRequests = [
    new CreateLot("Guernica", "by P. Picasso", 200_000_000m),
    new CreateLot("Starry night", "by V. van Gogh", 70_000_000m),
    new CreateLot("Barge Haulers on the Volga", "by I. Repin", 2_000_000m),
    new CreateLot("The Great Wave off Kanagawa", "by K. Hokusai", 2_760_000m),
    new CreateLot("The Kiss", "by G. Klimt", 240_000m),
    new CreateLot("Girl with a Pearl Earring", "by J. Vermeer", 30_000_000m)
];

var lotIds = lotsRequests.Select(async request => await mediator.Send(request)).Select(task => task.Result.Id).ToList();

var auction = await mediator.Send(new CreateAuction("Famous Worldwide paintings", DateTime.UtcNow, TimeSpan.FromMinutes(30), lotIds));

await mediator.Send(new CreateBid(auction.Id, lotIds[2], 5_000_000m));

await mediator.Send(new CreateBid(auction.Id, lotIds[2], 6_000_000m));

auction = await mediator.Send(new GetAuctionById(1));

Console.WriteLine(auction);