using AutoMapper;
using MediatR;
using VacationRental.Application.Models.Booking;
using VacationRental.Domain.Repositories;


namespace VacationRental.Application.CQRS.Queries.Booking
{
    public class GetBookingsForRentalQuery : IRequest<IReadOnlyList<BookingDto>>
    {
        public GetBookingsForRentalQuery(int rentalId)
        {
            RentalId = rentalId;
        }

        public GetBookingsForRentalQuery(int rentalId, DateTime start, DateTime end, int preparationTimeInDays)
        {
            RentalId = rentalId;
            Start = start;
            End = end;
            PreparationTimeInDays = preparationTimeInDays;
        }

        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int PreparationTimeInDays { get; set; }
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
            var bookings = (await _bookingRepository.GetByRentalId(request.RentalId))
                .Where(booking =>
                    (booking.Start <= request.Start.Date &&
                     booking.Start.AddDays(booking.Nights + request.PreparationTimeInDays) > request.Start.Date)
                    || (booking.Start < request.End &&
                        booking.Start.AddDays(booking.Nights + request.PreparationTimeInDays) >= request.End)
                    || (booking.Start > request.Start &&
                        booking.Start.AddDays(booking.Nights + request.PreparationTimeInDays) < request.End));

            return _mapper.Map<IReadOnlyList<BookingDto>>(bookings);
        }
    }
}
