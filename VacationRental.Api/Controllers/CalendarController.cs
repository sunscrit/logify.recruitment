using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Application.CQRS.Queries.Booking;
using VacationRental.Application.CQRS.Queries.Rental;
using VacationRental.Application.Models.Calendar;

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
        public async Task<ActionResult<CalendarDto>> GetAsync(int rentalId, DateTime start, int nights)
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

            var result = new CalendarDto
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateViewModel>()
            };

            var getBookingsForRentalQuery = new GetBookingsForRentalQuery(rental.Id);
            var bookingsForRental = await _mediator.Send(getBookingsForRentalQuery);

            var preparationTimeInDays = rental.PreparationTimeInDays;

            for (var i = 0; i < nights; i++)
            {
                var date = new CalendarDateViewModel
                {
                    Date = start.Date.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>(),
                    PreparationTimes = new List<PreparationTimeViewModel>()
                };

                foreach (var booking in bookingsForRental)
                {
                    var bookingEnd = booking.Start.AddDays(booking.Nights);
                    var preparationEnd = booking.Start.AddDays(booking.Nights + preparationTimeInDays);
                    var unitId = booking.Id % rental.Units + 1;
                    if (booking.Start <= date.Date && bookingEnd > date.Date)
                    {
                        date.Bookings.Add(new CalendarBookingViewModel { Id = booking.Id, Unit = unitId });
                    }
                    else if (bookingEnd <= date.Date && preparationEnd > date.Date)
                    {
                        date.PreparationTimes.Add(new PreparationTimeViewModel { Unit = unitId });
                    }
                }
                result.Dates.Add(date);
            }

            return Ok(result);
        }
    }
}
