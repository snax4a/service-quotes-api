﻿using Microsoft.EntityFrameworkCore;
using ServiceQuotes.Domain.Entities;
using ServiceQuotes.Infrastructure.Context.EntityConfigurations;

namespace ServiceQuotes.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerAddress> CustomerAddresses { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new System.ArgumentNullException(nameof(modelBuilder));
            }

            // todo fix/add account,customers,addresses configurations
            // custom entities configuration
            modelBuilder.ApplyConfiguration<Account>(new AccountEntityConfiguration());
            modelBuilder.ApplyConfiguration<CustomerAddress>(new CustomerAddressEntityConfiguration());
            modelBuilder.ApplyConfiguration<Employee>(new EmployeeEntityConfiguration());
            modelBuilder.ApplyConfiguration<Specialization>(new SpecializationEntityConfiguration());
            modelBuilder.ApplyConfiguration<EmployeeSpecialization>(new EmployeeSpecializationEntityConfiguration());
            modelBuilder.ApplyConfiguration<ServiceRequest>(new ServiceRequestEntityConfiguration());
            modelBuilder.ApplyConfiguration<ServiceRequestEmployee>(new ServiceRequestEmployeeEntityConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
