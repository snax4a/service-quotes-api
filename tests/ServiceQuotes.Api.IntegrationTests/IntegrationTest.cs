using ServiceQuotes.Api.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ServiceQuotes.Api.IntegrationTests
{
    public abstract class IntegrationTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        protected readonly WebApplicationFactory<Startup> _factory;

        public IntegrationTest(WebApplicationFactory<Startup> fixture)
        {
            _factory = fixture.BuildApplicationFactory();
        }
    }
}
