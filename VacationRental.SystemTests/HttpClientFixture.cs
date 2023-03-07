using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace VacationRental.SystemTests
{
    [CollectionDefinition("Integration")]
    public class HttpClientFixture : IDisposable
    {
        public HttpClient Client { get; private set; }
        public HttpClientFixture() => Client = new WebApplicationFactory<Program>().CreateClient();
        public void Dispose() => Client.Dispose();
    }
}
