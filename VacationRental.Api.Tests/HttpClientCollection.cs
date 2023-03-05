using Xunit;

namespace VacationRental.Api.Tests
{
    [CollectionDefinition("Integration")]
    public class HttpClientCollection : ICollectionFixture<HttpClientFixture> { }
}
