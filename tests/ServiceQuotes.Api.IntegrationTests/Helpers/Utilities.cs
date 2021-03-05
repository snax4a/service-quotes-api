using System;
using System.Collections.Generic;
using ServiceQuotes.Domain.Entities;
using ServiceQuotes.Infrastructure.Context;
using BC = BCrypt.Net.BCrypt;

namespace ServiceQuotes.Api.IntegrationTests.Helpers
{
    public static class Utilities
    {
        public static void InitializeDbForTests(AppDbContext db)
        {
            db.Heroes.AddRange(GetSeedingHeroes());
            db.Accounts.AddRange(GetSeedingAccounts());
            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(AppDbContext db)
        {
            db.Heroes.RemoveRange(db.Heroes);
            db.Accounts.RemoveRange(db.Accounts);
            InitializeDbForTests(db);
        }

        public static List<Hero> GetSeedingHeroes()
        {
            return new List<Hero>()
            {
                new Hero(){ Id = new Guid("824a7a65-b769-4b70-bccb-91f880b6ddf1"), Name = "Corban Best", HeroType = Domain.Entities.Enums.HeroType.ProHero },
                new Hero(){ Id = new Guid("b426070e-ccb3-42e6-8fb4-ef6aa5a62cc4"), Name = "Priya Hull", HeroType = Domain.Entities.Enums.HeroType.Student },
                new Hero(){ Id = new Guid("634769f7-a7b8-4146-9cb2-ff2dd90e886b"), Name = "Harrison Vu", HeroType = Domain.Entities.Enums.HeroType.Teacher }
            };
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
                    PasswordHash = BC.HashPassword("manager123"),
                },
                new Account()
                {
                    Id = new Guid("8720c542-7784-460a-91ca-31bf633eae50"),
                    Email = "customer@test.com",
                    Role = Domain.Entities.Enums.Role.Customer,
                    PasswordHash = BC.HashPassword("customer123"),
                },
                new Account()
                {
                    Id = new Guid("8411910e-5d47-40fb-a29b-6ad9dbbd9f63"),
                    Email = "employee@test.com",
                    Role = Domain.Entities.Enums.Role.Employee,
                    PasswordHash = BC.HashPassword("employee123")
                },
            };
        }


    }
}
