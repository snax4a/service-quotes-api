using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace ServiceQuotes.Api.IntegrationTests.Helpers
{
    public class FakeRemoteIpAddressMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IPAddress fakeIpAddress = IPAddress.Parse("123.123.1.23");

        public FakeRemoteIpAddressMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Connection.RemoteIpAddress = fakeIpAddress;

            await this.next(httpContext);
        }
    }
}
