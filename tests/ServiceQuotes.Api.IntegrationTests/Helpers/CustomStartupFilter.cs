using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;

namespace ServiceQuotes.Api.IntegrationTests.Helpers
{
    public class CustomStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                app.UseMiddleware<FakeRemoteIpAddressMiddleware>();
                next(app);
            };
        }
    }
}
