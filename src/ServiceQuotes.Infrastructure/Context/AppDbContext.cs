﻿using Microsoft.EntityFrameworkCore;
using ServiceQuotes.Domain.Entities;
using ServiceQuotes.Infrastructure.Context.EntityConfigurations;

namespace ServiceQuotes.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerAddress> CustomerAddresses { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<JobValuation> JobValuations { get; set; }
        public DbSet<ServiceRequestJobValuation> ServiceRequestJobValuations { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Payment> Payment { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new System.ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.ApplyConfiguration<Account>(new AccountEntityConfiguration());
            modelBuilder.ApplyConfiguration<RefreshToken>(new RefreshTokenEntityConfiguration());
            modelBuilder.ApplyConfiguration<CustomerAddress>(new CustomerAddressEntityConfiguration());
            modelBuilder.ApplyConfiguration<Employee>(new EmployeeEntityConfiguration());
            modelBuilder.ApplyConfiguration<Specialization>(new SpecializationEntityConfiguration());
            modelBuilder.ApplyConfiguration<EmployeeSpecialization>(new EmployeeSpecializationEntityConfiguration());
            modelBuilder.ApplyConfiguration<ServiceRequest>(new ServiceRequestEntityConfiguration());
            modelBuilder.ApplyConfiguration<ServiceRequestEmployee>(new ServiceRequestEmployeeEntityConfiguration());
            modelBuilder.ApplyConfiguration<Material>(new MaterialEntityConfiguration());
            modelBuilder.ApplyConfiguration<JobValuation>(new JobValuationEntityConfiguration());
            modelBuilder.ApplyConfiguration<ServiceRequestJobValuation>(new ServiceRequestJobValuationEntityConfiguration());
            modelBuilder.ApplyConfiguration<Quote>(new QuoteEntityConfiguration());
            modelBuilder.ApplyConfiguration<Payment>(new PaymentEntityConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
