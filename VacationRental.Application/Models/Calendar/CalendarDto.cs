namespace VacationRental.Application.Models.Calendar
{
    public class CalendarDto
    {
        public int RentalId { get; set; }
        public List<CalendarDateViewModel> Dates { get; set; }
    }
}
