using AutoMapper;
using VacationRental.Application.Models.Booking;
using VacationRental.Application.Models.Rental;
using VacationRental.Domain.Entities;

namespace VacationRental.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BookingEntity, BookingDto>();
            CreateMap<RentalEntity, RentalDto>();
        }
    }
}
