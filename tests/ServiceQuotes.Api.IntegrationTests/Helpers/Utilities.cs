using ServiceQuotes.Domain.Entities;
using ServiceQuotes.Infrastructure.Context;
using System;
using System.Collections.Generic;
using AppUtilities = ServiceQuotes.Application.Helpers.Utilities;

namespace ServiceQuotes.Api.IntegrationTests.Helpers
{
    public static class Utilities
    {
        public static void InitializeDbForTests(AppDbContext db)
        {
            db.Accounts.AddRange(GetSeedingAccounts());
            db.Customers.AddRange(GetSeedingCustomers());
            db.Employees.AddRange(GetSeedingEmployees());
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

        public static List<Customer> GetSeedingCustomers()
        {
            return new List<Customer>()
            {
                new Customer()
                {
                    Id = new Guid("12dbb416-6545-47a2-b553-fc54f93609f5"),
                    CompanyName = "Cinema City",
                    VatNumber = "PL7922298336",
                    AccountId = new Guid("8720c542-7784-460a-91ca-31bf633eae50")
                }
            };
        }

        public static List<Employee> GetSeedingEmployees()
        {
            return new List<Employee>()
            {
                new Employee()
                {
                    Id = new Guid("7c0e9b4c-b5a6-44a3-8f65-22141dc681cf"),
                    FirstName = "Szymon",
                    LastName = "Sus",
                    AccountId = new Guid("be95846f-6298-45eb-b732-dddadb2e4f83")
                },
                new Employee()
                {
                    Id = new Guid("f10b0d87-6e4a-4984-bf97-f8009b94be4d"),
                    FirstName = "Sebastian",
                    LastName = "Kośka",
                    AccountId = new Guid("8411910e-5d47-40fb-a29b-6ad9dbbd9f63")
                }
            };
        }
    }
}
