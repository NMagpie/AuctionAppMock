using AuctionApp.Application.Abstractions;
using AuctionApp.Application.Auctions.Responses;
using MediatR;

namespace AuctionApp.Application.Auctions.Queries;
public record GetAllAuctions() : IRequest<List<AuctionDto>>;

public class GetAllAuctionsHandler : IRequestHandler<GetAllAuctions, List<AuctionDto>>
{
    private readonly IAuctionRepository _auctionRepository;

    public GetAllAuctionsHandler(IAuctionRepository auctionRepository)
    {
        _auctionRepository = auctionRepository;
    }

    public Task<List<AuctionDto>> Handle(GetAllAuctions request, CancellationToken cancellationToken)
    {
        var auctions = _auctionRepository.GetAll();
        return Task.FromResult(auctions.Select(AuctionDto.FromAuction).ToList());
    }
}
