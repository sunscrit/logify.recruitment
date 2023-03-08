﻿using MediatR;
using VacationRental.Application.Models;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Repositories;

namespace VacationRental.Application.CQRS.Commands.Rental
{
    public class AddRentalCommand : IRequest<ResourceIdDto>
    {
        public int Units { get; set; }
        public int PreparationTimeInDays { get; set; }

        public AddRentalCommand(int units, int preparetionTimeInDays)
        {
            Units = units;
            PreparationTimeInDays = preparetionTimeInDays;
        }
    }

    public class AddRentalCommandHandler : IRequestHandler<AddRentalCommand, ResourceIdDto>
    {
        private readonly IRentalRepository _rentalRepository;

        public AddRentalCommandHandler(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        public async Task<ResourceIdDto> Handle(AddRentalCommand command, CancellationToken cancellationToken)
        {
            var rental = await _rentalRepository.AddAsync(new RentalEntity
            {
                Units = command.Units,
                PreparationTimeInDays = command.PreparationTimeInDays

            });
            return new ResourceIdDto { Id = rental.Id };
        }
    }
}
