using Newtonsoft.Json;
using System.Net.Http.Json;
using VacationRental.Application.Models;
using VacationRental.Application.Models.Rental;
using VacationRental.Domain.Entities;
using Xunit;

namespace VacationRental.SystemTests
{
    [Collection("Integration")]
    public class RentalTests
    {
        private readonly HttpClient _client;
        private readonly HttpClientFixture _fixture;

        public RentalTests(HttpClientFixture fixture)
        {
            _fixture = fixture;
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostRental_ThenAGetReturnsTheCreatedRental()
        {
            var units = 25;
            var preparationTimeInDays = 25;

            var postResult = await _fixture.CreateRental(units, preparationTimeInDays);

            using (var getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id}"))
            {
                Assert.True(getResponse.IsSuccessStatusCode);
                var getResult = JsonConvert.DeserializeObject<RentalEntity>(await getResponse.Content.ReadAsStringAsync());
                Assert.Equal(units, getResult.Units);
                Assert.Equal(preparationTimeInDays, getResult.PreparationTimeInDays);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostRental_ThenAGetWithNextIdReturnsNotFoumd()
        {
            var postResult = await _fixture.CreateRental(25,1);

            using (var getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id + 1}"))
            {
                Assert.False(getResponse.IsSuccessStatusCode);
            }
        }
    }
}
