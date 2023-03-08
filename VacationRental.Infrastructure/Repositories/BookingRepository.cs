using VacationRental.Domain.Entities;
using VacationRental.Domain.Repositories;

namespace VacationRental.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly IDictionary<int, BookingEntity> _bookings;

        public BookingRepository(IDictionary<int, BookingEntity> bookings)
        {
            _bookings = bookings;
        }

        public Task<bool> DoesExistAsync(int id)
        {
            return Task.FromResult(_bookings.ContainsKey(id));
        }

        public Task<BookingEntity> GetAsync(int id)
        {
            return Task.FromResult(_bookings[id]);
        }

        public Task<List<BookingEntity>> GetByRentalId(int rentalId)
        {
            return Task.FromResult(_bookings.Values.Where(b => b.RentalId == rentalId).ToList());
        }

        public Task<BookingEntity> AddAsync(BookingEntity booking)
        {
            booking.Id = _bookings.Keys.Count + 1;
            _bookings.Add(booking.Id, booking);
            return Task.FromResult(booking);
        }
    }
}
