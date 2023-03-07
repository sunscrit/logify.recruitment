using AutoMapper;
using MediatR;
using VacationRental.Application.Models;
using VacationRental.Domain.Repositories;


namespace VacationRental.Application.CQRS.Queries.Booking
{
    public class GetBookingsForRentalQuery : IRequest<IReadOnlyList<BookingDto>>
    {
        public GetBookingsForRentalQuery(int rentalId)
        {
            RentalId = rentalId;
        }

        public int RentalId { get; set; }
    }

    public class GetBookingsForRentalQueryHandler : IRequestHandler<GetBookingsForRentalQuery, IReadOnlyList<BookingDto>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public GetBookingsForRentalQueryHandler(IBookingRepository bookingRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<BookingDto>> Handle(GetBookingsForRentalQuery request, CancellationToken cancellationToken)
        {
            var bookings = await _bookingRepository.GetByRentalId(request.RentalId);
            return _mapper.Map<IReadOnlyList<BookingDto>>(bookings);
        }
    }
}
