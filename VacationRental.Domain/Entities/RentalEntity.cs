namespace VacationRental.Domain.Entities
{
    public class RentalEntity
    {
        public int Id { get; set; }
        public int Units { get; init; }
        public int PreparationTimeInDays { get; init; }
    }
}
