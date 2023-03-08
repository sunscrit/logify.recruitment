using System.Net.Http.Json;
using VacationRental.Application.Models;
using VacationRental.Application.Models.Booking;
using VacationRental.Application.Models.Calendar;
using VacationRental.Application.Models.Rental;
using Xunit;

namespace VacationRental.SystemTests
{
    [Collection("Integration")]
    public class GetCalendarTests
    {
        private readonly HttpClientFixture _fixture;
        private readonly HttpClient _client;

        public GetCalendarTests(HttpClientFixture fixture)
        {
            _fixture = fixture;
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenGetCalendar_ThenAGetReturnsTheCalculatedCalendar()
        {
            var postRentalResult = await _fixture.CreateRental(units: 2, preparationTimeInDays: 2);

            var postBooking1Result = await _fixture.CreateBooking(postRentalResult.Id, 2, new DateTime(2000, 01, 02));
            var postBooking2Result = await _fixture.CreateBooking(postRentalResult.Id, 2, new DateTime(2000, 01, 03));

            var nights = 6;
            using (var getCalendarResponse = await _client.GetAsync($"/api/v1/calendar?rentalId={postRentalResult.Id}&start=2000-01-01&nights={nights}"))
            {
                Assert.True(getCalendarResponse.IsSuccessStatusCode);

                var getCalendarResult = await getCalendarResponse.Content.ReadFromJsonAsync<CalendarDto>();

                Assert.Equal(postRentalResult.Id, getCalendarResult.RentalId);
                Assert.Equal(nights, getCalendarResult.Dates.Count);

                Assert.Equal(new DateTime(2000, 01, 01), getCalendarResult.Dates[0].Date);
                Assert.Empty(getCalendarResult.Dates[0].Bookings);
                Assert.Empty(getCalendarResult.Dates[0].PreparationTimes);

                Assert.Equal(new DateTime(2000, 01, 02), getCalendarResult.Dates[1].Date);
                Assert.Single(getCalendarResult.Dates[1].Bookings);
                Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Id == postBooking1Result.Id);
                Assert.Empty(getCalendarResult.Dates[1].PreparationTimes);

                Assert.Equal(new DateTime(2000, 01, 03), getCalendarResult.Dates[2].Date);
                Assert.Equal(2, getCalendarResult.Dates[2].Bookings.Count);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking1Result.Id);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking2Result.Id);
                Assert.Empty(getCalendarResult.Dates[2].PreparationTimes);

                Assert.Equal(new DateTime(2000, 01, 04), getCalendarResult.Dates[3].Date);
                Assert.Single(getCalendarResult.Dates[3].Bookings);
                Assert.Single(getCalendarResult.Dates[3].PreparationTimes);
                Assert.Contains(getCalendarResult.Dates[3].Bookings, x => x.Id == postBooking2Result.Id);

                Assert.Equal(new DateTime(2000, 01, 05), getCalendarResult.Dates[4].Date);
                Assert.Empty(getCalendarResult.Dates[4].Bookings);
                Assert.Equal(2, getCalendarResult.Dates[4].PreparationTimes.Count);

                Assert.Equal(new DateTime(2000, 01, 06), getCalendarResult.Dates[5].Date);
                Assert.Empty(getCalendarResult.Dates[5].Bookings);
                Assert.Single(getCalendarResult.Dates[5].PreparationTimes);
            }
        }


        [Fact]
        public async Task GivenInvalidRentalId_WhenGetCalendar_ThenBadRequest()
        {
            // Arrange
            var rentalId = -999;
            var start = new DateTime(1990, 03, 01);
            var nights = 3;

            // Act
            var response = await _client.GetAsync($"/api/v1/calendar?rentalId={rentalId}&start={start}&nights={nights}");

            // Assert
            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task GivenNegativeNights_WhenGetCalendar_ThenBadRequest()
        {
            // Arrange
            var rentalId = 1;
            var start = new DateTime(2023, 03, 01);
            var nights = -3;

            // Act
            var response = await _client.GetAsync($"/api/v1/calendar?rentalId={rentalId}&start={start}&nights={nights}");

            // Assert
            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task GivenZeroNights_WhenGetCalendar_ThenEmptyResponse()
        {
            // Arrange
            var rentalId = 1;
            var start = new DateTime(2023, 03, 01);
            var nights = 0;

            // Act
            var response = await _client.GetAsync($"/api/v1/calendar?rentalId={rentalId}&start={start}&nights={nights}");

            //Assert
            Assert.True(response.IsSuccessStatusCode);
            var result = await response.Content.ReadFromJsonAsync<CalendarDto>();
            Assert.Empty(result.Dates);
        }


        [Fact]
        public async Task GivenCompleteRequestWithBookingOneAfterAnother_WhenGetCalendar_ThenAGetReturnsTheCalculatedCalendar()
        {
            var postRentalResult = await _fixture.CreateRental(units: 2, preparationTimeInDays: 1);

            var postBooking1Result = await _fixture.CreateBooking(postRentalResult.Id, 4, new DateTime(2000, 01, 01));
            var postBooking2Result = await _fixture.CreateBooking(postRentalResult.Id, 2, new DateTime(2000, 01, 01));
            var postBooking3Result = await _fixture.CreateBooking(postRentalResult.Id, 1, new DateTime(2000, 01, 04));

            var nights = 6;
            using (var getCalendarResponse = await _client.GetAsync($"/api/v1/calendar?rentalId={postRentalResult.Id}&start=2000-01-01&nights={nights}"))
            {
                Assert.True(getCalendarResponse.IsSuccessStatusCode);

                var getCalendarResult = await getCalendarResponse.Content.ReadFromJsonAsync<CalendarDto>();

                Assert.Equal(postRentalResult.Id, getCalendarResult.RentalId);
                Assert.Equal(nights, getCalendarResult.Dates.Count);

                Assert.Equal(new DateTime(2000, 01, 01), getCalendarResult.Dates[0].Date);
                Assert.Equal(2, getCalendarResult.Dates[0].Bookings.Count);
                Assert.Contains(getCalendarResult.Dates[0].Bookings, x => x.Id == postBooking1Result.Id);
                Assert.Contains(getCalendarResult.Dates[0].Bookings, x => x.Id == postBooking2Result.Id);
                Assert.Empty(getCalendarResult.Dates[0].PreparationTimes);

                Assert.Equal(new DateTime(2000, 01, 02), getCalendarResult.Dates[1].Date);
                Assert.Equal(2, getCalendarResult.Dates[1].Bookings.Count);
                Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Id == postBooking1Result.Id);
                Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Id == postBooking2Result.Id);
                Assert.Empty(getCalendarResult.Dates[1].PreparationTimes);

                Assert.Equal(new DateTime(2000, 01, 03), getCalendarResult.Dates[2].Date);
                Assert.Single(getCalendarResult.Dates[2].Bookings);
                Assert.Single(getCalendarResult.Dates[2].PreparationTimes);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking1Result.Id);
                Assert.NotEqual(getCalendarResult.Dates[2].PreparationTimes.First().Unit, getCalendarResult.Dates[2].Bookings.First().Unit);

                Assert.Equal(new DateTime(2000, 01, 04), getCalendarResult.Dates[3].Date);
                Assert.Equal(2, getCalendarResult.Dates[3].Bookings.Count);
                Assert.Empty(getCalendarResult.Dates[3].PreparationTimes);
                Assert.Contains(getCalendarResult.Dates[3].Bookings, x => x.Id == postBooking1Result.Id);
                Assert.Contains(getCalendarResult.Dates[3].Bookings, x => x.Id == postBooking3Result.Id);
                Assert.NotEqual(getCalendarResult.Dates[3].Bookings[0].Unit, getCalendarResult.Dates[3].Bookings[1].Unit);

                Assert.Equal(new DateTime(2000, 01, 05), getCalendarResult.Dates[4].Date);
                Assert.Empty(getCalendarResult.Dates[4].Bookings);
                Assert.Equal(2, getCalendarResult.Dates[4].PreparationTimes.Count);
                Assert.NotEqual(getCalendarResult.Dates[4].PreparationTimes[0].Unit, getCalendarResult.Dates[4].PreparationTimes[1].Unit);

                Assert.Equal(new DateTime(2000, 01, 06), getCalendarResult.Dates[5].Date);
                Assert.Empty(getCalendarResult.Dates[5].Bookings);
                Assert.Empty(getCalendarResult.Dates[5].PreparationTimes);
            }
        }
    }
}
