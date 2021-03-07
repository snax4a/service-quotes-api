using ServiceQuotes.Domain.Entities;
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

            base.OnModelCreating(modelBuilder);
        }
    }
}
