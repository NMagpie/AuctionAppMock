using AuctionApp.Application.Abstractions;
using AuctionApp.Application.Lots.Responses;
using AuctionApp.Domain.Models;
using MediatR;

namespace AuctionApp.Application.Lots.Create;
public record CreateLot(string Title, string Description, decimal InitialPrice) : IRequest<LotDto>;

public class CreateLotHandler : IRequestHandler<CreateLot, LotDto>
{
    private readonly ILotRepository _lotRepository;

    public CreateLotHandler(ILotRepository lotRepository)
    {
        _lotRepository = lotRepository;
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

        var lot = new Lot()
        {
            Id = GetNextId(),
            Title = request.Title,
            Description = request.Description,
            InitialPrice = request.InitialPrice,
            Price = request.InitialPrice
        };

        var createdLot = _lotRepository.Create(lot);
        return Task.FromResult(LotDto.FromLot(createdLot));
    }

    private int GetNextId()
    {
        return _lotRepository.GetLastId();
    }
}
