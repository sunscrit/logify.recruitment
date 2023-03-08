using AutoMapper;
using MediatR;
using VacationRental.Application.Models.Calendar;
using VacationRental.Domain.Repositories;

namespace VacationRental.Application.CQRS.Queries.Booking
{
    public class GetCalendarQuery : IRequest<CalendarDto?>
    {
        public GetCalendarQuery(int rentalId)
        {
            RentalId = rentalId;
        }

        public int RentalId { get; set; }
    }

    public class GetCalendarQueryHandler : IRequestHandler<GetCalendarQuery, CalendarDto?>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRentalRepository _rentalRepository;
        private readonly IMapper _mapper;

        public GetCalendarQueryHandler(IBookingRepository bookingRepository, IRentalRepository rentalRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _rentalRepository = rentalRepository;
            _mapper = mapper;
        }

        public async Task<CalendarDto?> Handle(GetCalendarQuery request, CancellationToken cancellationToken)
        {
            if (!await _rentalRepository.DoesExistAsync(request.RentalId))
            {
                return null;
            }

            var rental = await _rentalRepository.GetAsync(request.RentalId);



            //return _mapper.Map<BookingDto>(booking);
            return new CalendarDto();
        }
    }
}
