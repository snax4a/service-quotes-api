using System;
using System.Collections.Generic;
using ServiceQuotes.Domain.Entities;
using ServiceQuotes.Infrastructure.Context;
using AppUtilities = ServiceQuotes.Application.Helpers.Utilities;

namespace ServiceQuotes.Api.IntegrationTests.Helpers
{
    public static class Utilities
    {
        public static void InitializeDbForTests(AppDbContext db)
        {
            db.Accounts.AddRange(GetSeedingAccounts());
            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(AppDbContext db)
        {
            db.Accounts.RemoveRange(db.Accounts);
            InitializeDbForTests(db);
        }

        public static List<Account> GetSeedingAccounts()
        {
            return new List<Account>()
            {
                new Account()
                {
                    Id = new Guid("be95846f-6298-45eb-b732-dddadb2e4f83"),
                    Email = "manager@test.com",
                    Role = Domain.Entities.Enums.Role.Manager,
                    PasswordHash = AppUtilities.HashPassword("manager123"),
                },
                new Account()
                {
                    Id = new Guid("8720c542-7784-460a-91ca-31bf633eae50"),
                    Email = "customer@test.com",
                    Role = Domain.Entities.Enums.Role.Customer,
                    PasswordHash = AppUtilities.HashPassword("customer123"),
                },
                new Account()
                {
                    Id = new Guid("8411910e-5d47-40fb-a29b-6ad9dbbd9f63"),
                    Email = "employee@test.com",
                    Role = Domain.Entities.Enums.Role.Employee,
                    PasswordHash = AppUtilities.HashPassword("employee123")
                },
            };
        }


    }
}
