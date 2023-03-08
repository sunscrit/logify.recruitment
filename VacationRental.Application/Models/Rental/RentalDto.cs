namespace VacationRental.Application.Models.Rental
{
    public record RentalDto
    {
        public int Id { get; set; }
        public int Units { get; set; }
        public int PreparationTimeInDays { get; set; }
    }
}
