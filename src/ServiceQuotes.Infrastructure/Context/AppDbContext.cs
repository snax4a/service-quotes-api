﻿using ServiceQuotes.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ServiceQuotes.Domain.Entities.Enums;
using System;
using BC = BCrypt.Net.BCrypt;

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

            modelBuilder.Entity<Account>().HasData(
                new Account
                {
                    Id = Guid.NewGuid(),
                    Email = "manager@service-quotes.com",
                    PasswordHash = BC.HashPassword("manager12345"),
                    Role = Role.Manager,
                    Created = DateTime.UtcNow
                }
            );

            // configure CustomerAddress many-to-many joining table composite primary key
            modelBuilder.Entity<CustomerAddress>().HasKey(ca => new { ca.CustomerId, ca.AddressId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
