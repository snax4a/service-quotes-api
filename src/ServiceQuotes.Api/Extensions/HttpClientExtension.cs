using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ServiceQuotes.Api.Extensions
{
    public static class HttpClientExtension
    {
        public static void AddPaynowHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient("paynow", c =>
            {
                c.BaseAddress = new Uri(configuration.GetValue<string>("Paynow.ApiUrl"));
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("Api-Key", configuration.GetValue<string>("Paynow.ApiKey"));
            });
        }
    }
}
