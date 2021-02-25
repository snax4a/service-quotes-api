using ServiceQuotes.Infrastructure.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceQuotes.Api.Extensions
{
    public static class DatabaseExtension
    {
        public static void AddApplicationDbContext(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {

            if (environment?.EnvironmentName == "Testing")
            {
                services.AddDbContextPool<AppDbContext>(o =>
                {
                    o.UseSqlite("Data Source=test.db");
                });
            }
            else
            {
                services.AddDbContextPool<AppDbContext>(o =>
                {
                    o.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                });
            }

        }
    }
}
