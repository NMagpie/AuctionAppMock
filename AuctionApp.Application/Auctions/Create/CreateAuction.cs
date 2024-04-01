using AuctionApp.Application.Abstractions;
using AuctionApp.Application.Auctions.Responses;
using AuctionApp.Domain.Enumerators;
using AuctionApp.Domain.Models;
using MediatR;

namespace AuctionApp.Application.Auctions.Create
{
    public record CreateAuction(string Title, DateTime TimeStart, TimeSpan Duration, List<int> LotIds) : IRequest<AuctionDto>;

    public class CreateAuctionHandler : IRequestHandler<CreateAuction, AuctionDto>
    {
        private readonly IAuctionRepository _auctionRepository;

        private readonly ILotRepository _lotRepository;

        public CreateAuctionHandler(IAuctionRepository auctionRepository, ILotRepository lotRepository)
        {
            _auctionRepository = auctionRepository;
            _lotRepository = lotRepository;
        }

        public Task<AuctionDto> Handle(CreateAuction request, 
            CancellationToken cancellationToken)
        {
            var lots = _lotRepository.GetLotsByIds(request.LotIds);
            if (lots.Count != request.LotIds.Count)
            {
                throw new ApplicationException("One or more lots not found");
            }

            var auction = new Auction()
            {
                Id = GetNextId(),
                Title = request.Title,
                TimeStart = request.TimeStart, 
                Duration = request.Duration, 
                Status = AuctionStatus.Created,
                Lots = lots 
            };

            var createdAuction = _auctionRepository.Create(auction);
            return Task.FromResult(AuctionDto.FromAuction(createdAuction));
        }

        private int GetNextId()
        {
            return _auctionRepository.GetLastId();
        }
    }
}
