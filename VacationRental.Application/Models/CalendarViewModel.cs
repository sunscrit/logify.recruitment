namespace VacationRental.Application.Models
{
    public class CalendarViewModel
    {
        public int RentalId { get; set; }
        public List<CalendarDateViewModel> Dates { get; set; }
    }
}
