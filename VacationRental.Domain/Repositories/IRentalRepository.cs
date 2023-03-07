using VacationRental.Domain.Entities;

namespace VacationRental.Domain.Repositories
{
    public interface IRentalRepository
    {
        Task<bool> DoesExistAsync(int rentalId);
        Task<RentalEntity> GetAsync(int rentalId);
        Task<RentalEntity> AddAsync(RentalEntity rental);
        Task<RentalEntity> UpdateAsync(RentalEntity rental);
    }
}
