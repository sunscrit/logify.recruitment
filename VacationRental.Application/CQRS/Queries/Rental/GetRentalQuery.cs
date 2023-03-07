using AutoMapper;
using MediatR;
using VacationRental.Application.Models;
using VacationRental.Domain.Repositories;

namespace VacationRental.Application.CQRS.Queries.Rental
{
    public class GetRentalQuery : IRequest<RentalDto?>
    {
        public GetRentalQuery(int rentalId)
        {
            RentalId = rentalId;
        }
        public int RentalId { get; set; }
    }

    public class GetRentalQueryHandler : IRequestHandler<GetRentalQuery, RentalDto?>
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IMapper _mapper;

        public GetRentalQueryHandler(IRentalRepository rentalRepository, IMapper mapper)
        {
            _rentalRepository = rentalRepository;
            _mapper = mapper;
        }

        public async Task<RentalDto?> Handle(GetRentalQuery request, CancellationToken cancellationToken)
        {
            if (!await _rentalRepository.DoesExistAsync(request.RentalId))
            {
                return null;
            }

            var rental = await _rentalRepository.GetAsync(request.RentalId);
            return _mapper.Map<RentalDto>(rental);
        }
    }
}
