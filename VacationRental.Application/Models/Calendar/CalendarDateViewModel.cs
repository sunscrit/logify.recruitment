namespace VacationRental.Application.Models.Calendar
{
    public class CalendarDateViewModel
    {
        public DateTime Date { get; set; }
        public List<CalendarBookingViewModel> Bookings { get; set; }
        public List<PreparationTimeViewModel> PreparationTimes { get; set; }
    }
}
