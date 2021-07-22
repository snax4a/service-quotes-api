using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceQuotes.Api.Extensions;
using ServiceQuotes.Api.Middleware;
using ServiceQuotes.Application.Helpers;
using ServiceQuotes.Application.Interfaces;
using ServiceQuotes.Application.Services;
using ServiceQuotes.Domain;
using ServiceQuotes.Domain.Repositories;
using ServiceQuotes.Infrastructure;
using ServiceQuotes.Infrastructure.Repositories;
using System.Linq;

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
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ISpecializationRepository, SpecializationRepository>();
            services.AddScoped<ISpecializationService, SpecializationService>();
            services.AddScoped<IQuoteRepository, QuoteRepository>();
            services.AddScoped<IQuoteService, QuoteService>();
            services.AddScoped<IServiceRequestRepository, ServiceRequestRepository>();
            services.AddScoped<IServiceRequestService, ServiceRequestService>();
            services.AddScoped<IMaterialRepository, MaterialRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IPaymentService, PaymentService>();

            // configure strongly typed settings object
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // WebApi Configuration
            services.AddControllers().AddNewtonsoftJson(
                options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

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

            // CORS configuration
            app.UseCors(policy =>
                policy
                  .WithOrigins(Configuration.GetValue<string>("AllowedOrigins").Split(";").ToArray())
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials()
            );

            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseHttpsRedirection();
            app.UseResponseCompression();
        }
    }
}
