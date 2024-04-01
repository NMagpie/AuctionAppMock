using AuctionApp.Application.Abstractions;
using AuctionApp.Application.Auctions.Responses;
using MediatR;

namespace AuctionApp.Application.Auctions.Queries;
public record GetAuctionById(int Id) : IRequest<AuctionDto>;

public class GetAuctionByIdHandler : IRequestHandler<GetAuctionById, AuctionDto>
{
    private readonly IAuctionRepository _auctionRepository;

    public GetAuctionByIdHandler(IAuctionRepository auctionRepository)
    {
        _auctionRepository = auctionRepository;
    }

    public Task<AuctionDto> Handle(GetAuctionById request, CancellationToken cancellationToken)
    {
        var auction = _auctionRepository.GetById(request.Id);
        if (auction == null)
        {
            throw new ApplicationException("Auction not found");
        }

        return Task.FromResult(AuctionDto.FromAuction(auction));
    }
}
