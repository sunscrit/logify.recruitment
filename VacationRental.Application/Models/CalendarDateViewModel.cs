namespace VacationRental.Application.Models
{
    public class CalendarDateViewModel
    {
        public DateTime Date { get; set; }
        public List<CalendarBookingEntity> Bookings { get; set; }
    }
}
