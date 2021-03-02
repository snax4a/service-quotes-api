using ServiceQuotes.Api.Extensions;
using ServiceQuotes.Application.Interfaces;
using ServiceQuotes.Application.Services;
using ServiceQuotes.Domain.Repositories;
using ServiceQuotes.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;
using ServiceQuotes.Application.Helpers;
using ServiceQuotes.Api.Middleware;

namespace ServiceQuotes.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Extension method for less clutter in startup
            services.AddApplicationDbContext(Configuration, Environment);

            //DI Services and Repos
            services.AddScoped<IHeroRepository, HeroRepository>();
            services.AddScoped<IHeroAppService, HeroAppService>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccountService, AccountService>();

            // configure strongly typed settings object
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // WebApi Configuration
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // for enum as strings
            });

            // AutoMapper settings
            services.AddAutoMapperSetup();

            // HttpContext for log enrichment
            services.AddHttpContextAccessor();

            // Swagger settings
            services.AddApiDoc();

            // GZip compression
            services.AddCompression();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // global error handler
            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseCustomSerilogRequestLogging();
            app.UseRouting();
            app.UseApiDoc();

            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //added request logging


            app.UseHttpsRedirection();


            app.UseResponseCompression();
        }
    }
}
