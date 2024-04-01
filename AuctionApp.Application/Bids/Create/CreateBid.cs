using AuctionApp.Application.Abstractions;
using AuctionApp.Application.Bids.Responses;
using AuctionApp.Domain.Models;
using MediatR;

namespace AuctionApp.Application.Lots.Create;
public record CreateBid(int AuctionId, int LotId, decimal Price) : IRequest<BidDto>;

public class CreateBidHandler : IRequestHandler<CreateBid, BidDto>
{
    private readonly IBidRepository _bidRepository;
    private readonly ILotRepository _lotRepository;
    private readonly IAuctionRepository _auctionRepository;

    public CreateBidHandler(
        IBidRepository bidRepository, 
        ILotRepository lotRepository, 
        IAuctionRepository auctionRepository)
    {
        _bidRepository = bidRepository;
        _lotRepository = lotRepository;
        _auctionRepository = auctionRepository;
    }

    public Task<BidDto> Handle(CreateBid request, CancellationToken cancellationToken)
    {
        var auction = _auctionRepository.GetById(request.AuctionId);
        if (auction == null)
        {
            throw new ApplicationException("Auction not found");
        }

        var lot = _lotRepository.GetById(request.LotId);
        if (lot == null)
        {
            throw new ApplicationException("Lot not found");
        }

        if (lot.Price >= request.Price)
        {
            throw new ApplicationException("Price cannot be lower than latest price");
        }

        lot.Price = request.Price;

        var bid = new Bid()
        {
            Id = GetNextId(),
            Auction = auction,
            Lot = lot,
            Price = request.Price,
            Time = DateTime.UtcNow,
        };

        _lotRepository.AddBidById(lot.Id, bid);

        var createdBid = _bidRepository.Create(bid);
        return Task.FromResult(BidDto.FromBid(createdBid));
    }

    private int GetNextId()
    {
        return _bidRepository.GetLastId();
    }
}
