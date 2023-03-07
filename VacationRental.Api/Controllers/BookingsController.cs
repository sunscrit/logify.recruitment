using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Application.CQRS.Commands.Booking;
using VacationRental.Application.CQRS.Queries.Booking;
using VacationRental.Application.CQRS.Queries.Rental;
using VacationRental.Application.Models;

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
        public async Task<ActionResult<ResourceIdDto>> PostAsync(BookingBindingModel model)
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

            var getBookingsForRentalQuery = new GetBookingsForRentalQuery(rental.Id);
            var bookingsForRental = await _mediator.Send(getBookingsForRentalQuery);

            //if (!IsAvailable(bookingsForRental, startDate, endDate, rental.Units))
            //{
            //    return Conflict("Not available");
            //}

            for (var i = 0; i < model.Nights; i++)
            {
                var count = 0;
                foreach (var booking in bookingsForRental)
                {
                    if ((booking.Start <= model.Start.Date && booking.Start.AddDays(booking.Nights) > model.Start.Date)
                        || (booking.Start < model.Start.AddDays(model.Nights) && booking.Start.AddDays(booking.Nights) >= model.Start.AddDays(model.Nights))
                        || (booking.Start > model.Start && booking.Start.AddDays(booking.Nights) < model.Start.AddDays(model.Nights)))
                    {
                        count++;
                    }
                }
                if (count >= rental.Units)
                {
                    return Conflict("Not available");
                }
            }

            var addBookingCommand = new AddBookingCommand(model.RentalId, model.Nights, model.Start.Date);
            return Ok(await _mediator.Send(addBookingCommand));
        }

        private bool IsAvailable(IEnumerable<BookingDto> bookings, DateTime startDate, DateTime endDate, int units)
        {
            var count = 0;
            foreach (var booking in bookings)
            {
                if (booking.Start <= startDate && booking.Start.AddDays(booking.Nights) > startDate
                    || booking.Start < endDate && booking.Start.AddDays(booking.Nights) >= endDate
                    || booking.Start > startDate && booking.Start.AddDays(booking.Nights) < endDate)
                {
                    count++;
                }
                if (count >= units)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
