using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ServiceQuotes.Api.Extensions
{
    public static class HttpClientExtension
    {
        public static void AddPaynowHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            string apiUrl = configuration.GetValue<string>("AppSettings:PaynowApiUrl");
            string apiKey = configuration.GetValue<string>("AppSettings:PaynowApiKey");

            if (string.IsNullOrEmpty(apiUrl) || string.IsNullOrEmpty(apiKey))
                throw new Exception("Paynow ApiUrl and ApiKey configuration values can not be empty.");

            services.AddHttpClient("paynow", c =>
            {
                c.BaseAddress = new Uri(apiUrl);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("Api-Key", apiKey);
            });
        }
    }
}
