using System.Collections.Concurrent;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Repositories;
using VacationRental.Infrastructure.Repositories;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<IDictionary<int, RentalEntity>>(new ConcurrentDictionary<int, RentalEntity>());
        services.AddSingleton<IDictionary<int, BookingEntity>>(new ConcurrentDictionary<int, BookingEntity>());
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IRentalRepository, RentalRepository>();

        return services;
    }
}