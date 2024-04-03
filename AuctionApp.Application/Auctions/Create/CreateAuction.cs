using AuctionApp.Application.Abstractions;
using AuctionApp.Application.Auctions.Responses;
using AuctionApp.Domain.Enumerators;
using AuctionApp.Domain.Models;
using MediatR;

namespace AuctionApp.Application.Auctions.Create;
public record CreateAuction(string Title, DateTime TimeStart, TimeSpan Duration) : IRequest<AuctionDto>;

public class CreateAuctionHandler : IRequestHandler<CreateAuction, AuctionDto>
{
    private readonly IAuctionRepository _auctionRepository;

    public CreateAuctionHandler(IAuctionRepository auctionRepository, ILotRepository lotRepository)
    {
        _auctionRepository = auctionRepository;
    }

    public Task<AuctionDto> Handle(CreateAuction request,
        CancellationToken cancellationToken)
    {
        if (request.Title is null || request.Title?.Length == 0)
        {
            throw new ApplicationException("Title Required");
        }

        if (request.TimeStart <= DateTime.UtcNow)
        {
            throw new ApplicationException("Cannot set already passed date");
        }

        if (request.Duration < TimeSpan.FromMinutes(1) ||
            request.Duration > TimeSpan.FromHours(5)
            )
        {
            throw new ApplicationException("Auction cannot durate less than 1 minute and more than 5 hours");
        }

        var auction = new Auction()
        {
            Id = GetNextId(),
            Title = request.Title,
            TimeStart = request.TimeStart,
            Duration = request.Duration,
            Status = AuctionStatus.Created,
            Lots = [],
            Bids = [],
        };

        var createdAuction = _auctionRepository.Create(auction);
        return Task.FromResult(AuctionDto.FromAuction(createdAuction));
    }

    private int GetNextId()
    {
        return _auctionRepository.GetLastId();
    }
}
