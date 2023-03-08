namespace VacationRental.Application.Models.Rental
{
    public record RentalRequest
    {
        public int Units { get; set; }
        public int PreparationTimeInDays { get; set; }
    }
}
