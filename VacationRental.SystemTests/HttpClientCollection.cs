using Xunit;

namespace VacationRental.SystemTests
{
    [CollectionDefinition("Integration")]
    public class HttpClientCollection : ICollectionFixture<HttpClientFixture> { }
}
