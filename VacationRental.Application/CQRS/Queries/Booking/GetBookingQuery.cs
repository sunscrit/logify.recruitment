using AutoMapper;
using MediatR;
using VacationRental.Application.Models;
using VacationRental.Domain.Repositories;

namespace VacationRental.Application.CQRS.Queries.Booking
{
    public class GetBookingQuery : IRequest<BookingDto?>
    {
        public GetBookingQuery(int bookingId)
        {
            BookingId = bookingId;
        }

        public int BookingId { get; set; }
    }

    public class GetBookingQueryHandler : IRequestHandler<GetBookingQuery, BookingDto?>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public GetBookingQueryHandler(IBookingRepository bookingRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        public async Task<BookingDto?> Handle(GetBookingQuery request, CancellationToken cancellationToken)
        {
            if (!await _bookingRepository.DoesExistAsync(request.BookingId))
            {
                return null;
            }

            var booking = await _bookingRepository.GetAsync(request.BookingId);

            return _mapper.Map<BookingDto>(booking);
        }
    }
}
