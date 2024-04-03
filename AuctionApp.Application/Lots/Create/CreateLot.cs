using AuctionApp.Application.Abstractions;
using AuctionApp.Application.Lots.Responses;
using AuctionApp.Domain.Models;
using MediatR;

namespace AuctionApp.Application.Lots.Create;
public record CreateLot(string Title, string Description, int AuctionId, decimal InitialPrice) : IRequest<LotDto>;

public class CreateLotHandler : IRequestHandler<CreateLot, LotDto>
{
    private readonly ILotRepository _lotRepository;

    private readonly IAuctionRepository _auctionRepository;

    public CreateLotHandler(ILotRepository lotRepository, IAuctionRepository auctionRepository)
    {
        _lotRepository = lotRepository;
        _auctionRepository = auctionRepository;
    }

    public Task<LotDto> Handle(CreateLot request, CancellationToken cancellationToken)
    {
        if (request.Title is null || request.Title?.Length == 0)
        {
            throw new ApplicationException("Title Required");
        }

        if (request.Description is null || request.Description?.Length == 0)
        {
            throw new ApplicationException("Description Required");
        }

        if (request.InitialPrice <= 0)
        {
            throw new ApplicationException("Price must be positive");
        }

        var auction = _auctionRepository.GetById(request.AuctionId);
        if (auction == null)
        {
            throw new ApplicationException("Auction not found");
        }

        var lot = new Lot()
        {
            Id = GetNextId(),
            Title = request.Title,
            Description = request.Description,
            Auction = auction,
            InitialPrice = request.InitialPrice,
            Price = request.InitialPrice
        };

        var createdLot = _lotRepository.Create(lot);

        _auctionRepository.AddLotById(auction.Id, createdLot);

        return Task.FromResult(LotDto.FromLot(createdLot));
    }

    private int GetNextId()
    {
        return _lotRepository.GetLastId();
    }
}
