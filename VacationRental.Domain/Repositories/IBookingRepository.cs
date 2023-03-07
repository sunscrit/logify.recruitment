using VacationRental.Domain.Entities;

namespace VacationRental.Domain.Repositories
{
    public interface IBookingRepository
    {
        Task<bool> DoesExistAsync(int id);
        Task<BookingEntity> GetAsync(int id);
        Task<List<BookingEntity>> GetByRentalId(int rentalId);
        Task<BookingEntity> AddAsync(BookingEntity booking);
    }
}
