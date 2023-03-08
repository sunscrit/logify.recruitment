using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Application.CQRS.Commands.Booking;
using VacationRental.Application.CQRS.Queries.Booking;
using VacationRental.Application.CQRS.Queries.Rental;
using VacationRental.Application.Models;
using VacationRental.Application.Models.Booking;
using VacationRental.Application.Models.Rental;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public async Task<ActionResult<BookingDto>> GetAsync(int bookingId)
        {
            var query = new GetBookingQuery(bookingId);
            var queryResult = await _mediator.Send(query);

            return queryResult == null ? NotFound("Booking not found") : Ok(queryResult);
        }

        [HttpPost]
        public async Task<ActionResult<ResourceIdDto>> PostAsync(BookingRequest model)
        {
            if (model.Nights <= 0)
            {
                return BadRequest("Nights must be positive");
            }

            var query = new GetRentalQuery(model.RentalId);
            var rental = await _mediator.Send(query);

            if (rental == null)
            {
                return NotFound("Rental not found");
            }

            var modelEnd = model.Start.AddDays(model.Nights + rental.PreparationTimeInDays);

            var getBookingsForRentalQuery = new GetBookingsForRentalQuery(rental.Id, model.Start, modelEnd, rental.PreparationTimeInDays);
            var bookingsForRental = await _mediator.Send(getBookingsForRentalQuery);

            if (bookingsForRental.Count >= rental.Units)
            {
                return Conflict("Not available");
            }

            var addBookingCommand = new AddBookingCommand(model.RentalId, model.Nights, model.Start.Date, AvaliableUnitId(bookingsForRental, rental));
            return Ok(await _mediator.Send(addBookingCommand));
        }

        private static int AvaliableUnitId(IReadOnlyList<BookingDto> bookingsForRental, RentalDto rental)
        {
            var bookedUnits = bookingsForRental.Select(x => x.Unit).ToList();
            return Enumerable.Range(1, rental.Units).Except(bookedUnits).FirstOrDefault();
        }
    }
}
