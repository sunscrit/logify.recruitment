using Microsoft.AspNetCore.Mvc.Testing;
using VacationRental.Application.Models.Rental;
using VacationRental.Application.Models;
using Xunit;
using System.Net.Http.Json;

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
    }
}
