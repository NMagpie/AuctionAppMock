using AuctionApp.Application.Abstractions;
using AuctionApp.Application.Auctions.Responses;
using MediatR;

namespace AuctionApp.Application.Auctions.Create;
public record AddLot(int LotId, int AuctionId) : IRequest<AuctionDto>;

public class AddLotHandler : IRequestHandler<AddLot, AuctionDto>
{
    private readonly IAuctionRepository _auctionRepository;

    private readonly ILotRepository _lotRepository;

    public AddLotHandler(IAuctionRepository auctionRepository, ILotRepository lotRepository)
    {
        _auctionRepository = auctionRepository;
        _lotRepository = lotRepository;
    }

    public Task<AuctionDto> Handle(AddLot request,
        CancellationToken cancellationToken)
    {
        var lot = _lotRepository.GetById(request.LotId);
        if (lot == null)
        {
            throw new ApplicationException("Lot not found");
        }

        var auction = _auctionRepository.GetById(request.AuctionId);
        if (auction == null)
        {
            throw new ApplicationException("Auction not found");
        }

        auction.Lots.Add(lot);

        return Task.FromResult(AuctionDto.FromAuction(auction));
    }
}
