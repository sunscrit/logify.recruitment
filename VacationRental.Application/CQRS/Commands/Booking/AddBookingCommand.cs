using MediatR;
using VacationRental.Application.Models;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Repositories;

namespace VacationRental.Application.CQRS.Commands.Booking
{
    public class AddBookingCommand : IRequest<ResourceIdDto>
    {
        public int RentalId { get; set; }
        public int Nights { get; set; }
        public DateTime Start { get; set; }
        public int Unit { get; set; }

        public AddBookingCommand(int rentalId, int nights, DateTime start, int unit)
        {
            RentalId = rentalId;
            Nights = nights;
            Start = start;
            Unit = unit;
        }
    }

    public class AddBookingCommandHandler : IRequestHandler<AddBookingCommand, ResourceIdDto>
    {
        private readonly IBookingRepository _bookingRepository;

        public AddBookingCommandHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<ResourceIdDto> Handle(AddBookingCommand command, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.AddAsync(new BookingEntity
            {
                RentalId = command.RentalId,
                Nights = command.Nights,
                Start = command.Start,
                Unit = command.Unit,
            });
            return new ResourceIdDto { Id = booking.Id };
        }
    }
}
