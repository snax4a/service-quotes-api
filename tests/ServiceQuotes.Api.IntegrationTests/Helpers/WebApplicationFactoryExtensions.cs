using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServiceQuotes.Application.Helpers;
using ServiceQuotes.Infrastructure.Context;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using AppUtilities = ServiceQuotes.Application.Helpers.Utilities;

namespace ServiceQuotes.Api.IntegrationTests.Helpers
{
    public static class WebApplicationFactoryExtensions
    {
        public const string JWTSecret = "testing-jwt-secret";
        private static readonly object _lock = new object();
        private static bool _databaseInitialized;

        public static WebApplicationFactory<TStartup> BuildApplicationFactory<TStartup>(this WebApplicationFactory<TStartup> factory) where TStartup : class
        {
            return factory.WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
                builder.ConfigureServices(services =>
                {
                    var sp = services.BuildServiceProvider();

                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<AppDbContext>();
                        var logger = scopedServices
                            .GetRequiredService<ILogger<WebApplicationFactory<TStartup>>>();

                        lock (_lock)
                        {
                            if (!_databaseInitialized)
                            {
                                db.Database.EnsureDeleted();
                                db.Database.EnsureCreated();

                                try
                                {
                                    Utilities.InitializeDbForTests(db);
                                    _databaseInitialized = true;
                                }
                                catch (Exception ex)
                                {
                                    logger.LogError(ex, "An error occurred seeding the " +
                                                        "database with test messages. Error: {Message}", ex.Message);
                                }
                            }
                        }
                    }
                });

                builder.ConfigureTestServices(services =>
                {
                    services.Configure<AppSettings>(myOptions =>
                    {
                        myOptions.Secret = JWTSecret;
                    });

                    // used to extend middleware pipeline with FakeRemoteIpAddressMiddleware
                    services.AddSingleton<IStartupFilter, CustomStartupFilter>();
                });
            });
        }


        public static WebApplicationFactory<TStartup> RebuildDb<TStartup>(this WebApplicationFactory<TStartup> factory) where TStartup : class
        {
            return factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var serviceProvider = services.BuildServiceProvider();

                    using (var scope = serviceProvider.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices
                            .GetRequiredService<AppDbContext>();
                        var logger = scopedServices
                            .GetRequiredService<ILogger<IntegrationTest>>();
                        try
                        {
                            Utilities.ReinitializeDbForTests(db);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "An error occurred seeding " +
                                                "the database with test messages. Error: {Message}",
                                ex.Message);
                        }
                    }
                });
            });
        }

        // Creates HttpClient with authorization header containing generated JWT from accountId
        public static HttpClient CreateClientWithAuth<TStartup>(this WebApplicationFactory<TStartup> factory, Guid accountId) where TStartup : class
        {
            var client = factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                }
            );

            string token = AppUtilities.GenerateJwtToken(accountId, JWTSecret);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return client;
        }
    }
}
