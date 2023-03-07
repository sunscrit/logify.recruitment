using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Application.CQRS.Queries.Booking;
using VacationRental.Application.CQRS.Queries.Rental;
using VacationRental.Application.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CalendarController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<CalendarViewModel>> GetAsync(int rentalId, DateTime start, int nights)
        {
            if (nights < 0)
            {
                return BadRequest("Nights must be positive");
            }

            var query = new GetRentalQuery(rentalId);
            var rental = await _mediator.Send(query);

            if (rental == null)
            {
                return NotFound("Rental not found");
            }

            var result = new CalendarViewModel
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateViewModel>()
            };

            var getBookingsForRentalQuery = new GetBookingsForRentalQuery(rental.Id);
            var bookingsForRental = await _mediator.Send(getBookingsForRentalQuery);

            for (var i = 0; i < nights; i++)
            {
                var date = new CalendarDateViewModel
                {
                    Date = start.Date.AddDays(i),
                    Bookings = new List<CalendarBookingEntity>()
                };

                foreach (var booking in bookingsForRental)
                {
                    if (booking.Start <= date.Date && booking.Start.AddDays(booking.Nights) > date.Date)
                    {
                        date.Bookings.Add(new CalendarBookingEntity { Id = booking.Id });
                    }
                }

                result.Dates.Add(date);
            }

            return Ok(result);
        }
    }
}
