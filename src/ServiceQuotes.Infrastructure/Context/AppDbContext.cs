using ServiceQuotes.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace ServiceQuotes.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerAddress> CustomerAddresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new System.ArgumentNullException(nameof(modelBuilder));
            }

            // custom entities configuration
            modelBuilder.ApplyConfiguration<Account>(new AccountEntityConfiguration());
            modelBuilder.ApplyConfiguration<CustomerAddress>(new CustomerAddressEntityConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
