namespace VacationRental.Domain.Entities
{
    public class BookingEntity
    {
        public int Id { get; set; }
        public int RentalId { get; init; }
        public DateTime Start { get; init; }
        public int Nights { get; init; }
        public int Unit { get; init; }
    }
}
