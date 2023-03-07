using Newtonsoft.Json;
using System.Net.Http.Json;
using VacationRental.Application.Models;
using VacationRental.Domain.Entities;
using Xunit;

namespace VacationRental.SystemTests
{
    [Collection("Integration")]
    public class RentalTests
    {
        private readonly HttpClient _client;

        public RentalTests(HttpClientFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostRental_ThenAGetReturnsTheCreatedRental()
        {
            var units = 25;
            var postResult = await CreateRental(25);

            using (var getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id}"))
            {
                Assert.True(getResponse.IsSuccessStatusCode);

                var getResult = JsonConvert.DeserializeObject<RentalEntity>(await getResponse.Content.ReadAsStringAsync());
                Assert.Equal(units, getResult.Units);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostRental_ThenAGetWithNextIdReturnsNotFoumd()
        {
            var postResult = await CreateRental(25);

            using (var getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id + 1}"))
            {
                Assert.False(getResponse.IsSuccessStatusCode);
            }
        }

        public async Task<ResourceIdDto> CreateRental(int units)
        {
            var request = new RentalBindingModel
            {
                Units = units
            };

            ResourceIdDto postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = JsonConvert.DeserializeObject<ResourceIdDto>(await postResponse.Content.ReadAsStringAsync());
            }

            return postResult;
        }
    }
}
