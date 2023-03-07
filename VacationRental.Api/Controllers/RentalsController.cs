using MediatR;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Application.CQRS.Commands.Rental;
using VacationRental.Application.CQRS.Queries.Rental;
using VacationRental.Application.Models;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RentalsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public async Task<ActionResult<RentalDto>> GetAsync(int rentalId)
        {
            var query = new GetRentalQuery(rentalId);
            var queryResult = await _mediator.Send(query);

            return queryResult == null ? NotFound("Rental not found") : Ok(queryResult);
        }

        [HttpPost]
        public async Task<ActionResult<ResourceIdDto>> PostAsync(RentalBindingModel model)
        {
            var createCommand = new AddRentalCommand(model.Units);
            return Ok(await _mediator.Send(createCommand));
        }
    }
}
