using Microsoft.AspNetCore.Mvc.Testing;
using VacationRental.Application.Models.Rental;
using VacationRental.Application.Models;
using Xunit;
using System.Net.Http.Json;
using VacationRental.Application.Models.Booking;

namespace VacationRental.SystemTests
{
    [CollectionDefinition("Integration")]
    public class HttpClientFixture : IDisposable
    {
        public HttpClient Client { get; private set; }
        public HttpClientFixture() => Client = new WebApplicationFactory<Program>().CreateClient();
        public void Dispose() => Client.Dispose();

        public async Task<ResourceIdDto> CreateRental(int units, int preparationTimeInDays)
        {
            var postRentalRequest = new RentalRequest
            {
                Units = units,
                PreparationTimeInDays = preparationTimeInDays
            };

            ResourceIdDto postRentalResult;
            using (var postRentalResponse = await Client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadFromJsonAsync<ResourceIdDto>();
            }

            return postRentalResult;
        }

        public async Task<ResourceIdDto?> CreateBooking(int rentalId, int nights, DateTime start)
        {
            var postBooking1Request = new BookingRequest
            {
                RentalId = rentalId,
                Nights = nights,
                Start = start
            };

            ResourceIdDto postBookingResult;
            using (var postBooking1Response = await Client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
                postBookingResult = await postBooking1Response.Content.ReadFromJsonAsync<ResourceIdDto>();
            }

            return postBookingResult;
        }
    }
}
