using VacationRental.Domain.Entities;
using VacationRental.Domain.Repositories;

namespace VacationRental.Infrastructure.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly IDictionary<int, RentalEntity> _rentals;

        public RentalRepository(IDictionary<int, RentalEntity> rentals)
        {
            _rentals = rentals;
        }

        public Task<bool> DoesExistAsync(int rentalId)
        {
            return Task.FromResult(_rentals.ContainsKey(rentalId));
        }

        public Task<RentalEntity> GetAsync(int rentalId)
        {
            return Task.FromResult(_rentals[rentalId]);
        }

        public Task<RentalEntity> AddAsync(RentalEntity rental)
        {
            rental.Id = _rentals.Keys.Count + 1;
            _rentals.Add(rental.Id, rental);
            return Task.FromResult(rental);
        }

        public Task<RentalEntity> UpdateAsync(RentalEntity rental)
        {
            _rentals[rental.Id] = rental;
            return Task.FromResult(rental);
        }
    }
}
