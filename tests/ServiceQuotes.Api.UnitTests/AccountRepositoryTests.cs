using ServiceQuotes.Domain.Entities;
using ServiceQuotes.Infrastructure.Context;
using ServiceQuotes.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using AppUtilities = ServiceQuotes.Application.Helpers.Utilities;

namespace ServiceQuotes.Api.UnitTests
{
    public class AccountRepositoryTests
    {
        private AppDbContext CreateDbContext(string name)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(name)
                .Options;
            return new AppDbContext(options);
        }

        [Theory]
        [InlineData("3de1ef99-f264-4149-93a8-870c855524e5")]
        [InlineData("546ab9db-39fd-469e-99a8-2175e743be2c")]
        [InlineData("ca36ef1c-0d0e-4179-9c13-76b4d48d4f6b")]
        [InlineData("fa6760c7-5f8a-49f8-8676-a8cea638b108")]
        [InlineData("669f1169-ce8d-46fb-b549-06efb03448c7")]
        public async Task GetById_existing_Accounts(Guid id)
        {
            // Arrange
            using (var context = CreateDbContext("GetById_existing_Accounts"))
            {
                context.Set<Account>().Add(new Account { Id = id });
                await context.SaveChangesAsync();
            }
            Account account = null;

            // Act
            using (var context = CreateDbContext("GetById_existing_Accounts"))
            {
                var repository = new AccountRepository(context);
                account = await repository.GetById(id);
            }

            // Assert
            account.Should().NotBeNull();
            account.Id.Should().Be(id);
        }

        [Theory]
        [InlineData("3de1ef99-f264-4149-93a8-870c855524e5")]
        [InlineData("546ab9db-39fd-469e-99a8-2175e743be2c")]
        [InlineData("ca36ef1c-0d0e-4179-9c13-76b4d48d4f6b")]
        [InlineData("fa6760c7-5f8a-49f8-8676-a8cea638b108")]
        [InlineData("669f1169-ce8d-46fb-b549-06efb03448c7")]
        public async Task GetById_nonexistent_Accounts(Guid id)
        {
            // Arrange
            using (var context = CreateDbContext("GetById_nonexistent_Accounts"))
            {
                context.Set<Account>().Add(new Account { Id = id });
                await context.SaveChangesAsync();
            }
            Account account = null;

            // Act
            using (var context = CreateDbContext("GetById_nonexistent_Accounts"))
            {
                var repository = new AccountRepository(context);
                account = await repository.GetById(new Guid());
            }

            // Assert
            account.Should().BeNull();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(5)]
        public async Task GetAll_Accounts(int count)
        {
            // Arrange
            using (var context = CreateDbContext($"GetAll_with_accounts_{count}"))
            {
                for (var i = 0; i < count; i++)
                {
                    context.Set<Account>().Add(new Account());
                }
                await context.SaveChangesAsync();
            }
            List<Account> accounts = null;

            // Act
            using (var context = CreateDbContext($"GetAll_with_accounts_{count}"))
            {
                var repository = new AccountRepository(context);
                accounts = await repository.GetAll().ToListAsync();
            }

            // Assert
            accounts.Should().NotBeNull();
            accounts.Count().Should().Be(count);
        }

        [Fact]
        public async Task Create_Account()
        {
            // Arrange
            int result;

            // Act
            var account = new Account()
            {
                Email = "test.customer@test.com",
                PasswordHash = AppUtilities.HashPassword("test123"),
                Role = Domain.Entities.Enums.Role.Customer
            };

            using (var context = CreateDbContext("Create_Account"))
            {
                var repository = new AccountRepository(context);
                repository.Create(account);
                result = await repository.SaveChangesAsync();
            }

            // Assert
            result.Should().BeGreaterThan(0);
            result.Should().Be(1);
            // Simulate access from another context to verifiy that correct data was saved to database
            using (var context = CreateDbContext("Create_Account"))
            {
                (await context.Accounts.CountAsync()).Should().Be(1);
                (await context.Accounts.FirstAsync()).Should().Be(account);
            }
        }

        [Fact]
        public async Task Update_Account()
        {
            // Arrange
            int result;
            Guid? id;
            using (var context = CreateDbContext("Update_Account"))
            {
                var accounts = new List<Account>()
                {
                    new Account()
                    {
                        Email = "first.account@test.com",
                        PasswordHash = AppUtilities.HashPassword("test123"),
                        Role = Domain.Entities.Enums.Role.Employee
                    },
                    new Account()
                    {
                        Email = "second.account@test.com",
                        PasswordHash = AppUtilities.HashPassword("test1234"),
                        Role = Domain.Entities.Enums.Role.Customer
                    }
                };
                context.Set<Account>().AddRange(accounts);
                await context.SaveChangesAsync();
                id = accounts.First().Id;
            }

            // Act
            Account updateAccount;
            using (var context = CreateDbContext("Update_Account"))
            {
                updateAccount = await context.Set<Account>().FirstOrDefaultAsync(x => x.Id == id);
                updateAccount.Role = Domain.Entities.Enums.Role.Customer;
                updateAccount.Email = "first.updated@test.com";
                var repository = new AccountRepository(context);
                repository.Update(updateAccount);
                result = await repository.SaveChangesAsync();
            }

            // Assert
            result.Should().Be(1);
            // Simulate access from another context to verifiy that correct data was saved to database
            using (var context = CreateDbContext("Update_Account"))
            {
                (await context.Accounts.FirstAsync(x => x.Id == updateAccount.Id)).Should().Be(updateAccount);
            }
        }

        [Fact]
        public async Task Delete_Account()
        {
            // Arrange
            int result;
            Guid? id;
            using (var context = CreateDbContext("Delete_Account"))
            {
                var accounts = new List<Account>()
                {
                    new Account()
                    {
                        Email = "first.account@test.com",
                        PasswordHash = AppUtilities.HashPassword("test123"),
                        Role = Domain.Entities.Enums.Role.Employee
                    },
                    new Account()
                    {
                        Email = "second.account@test.com",
                        PasswordHash = AppUtilities.HashPassword("test1234"),
                        Role = Domain.Entities.Enums.Role.Customer
                    }
                };
                context.Set<Account>().AddRange(accounts);
                await context.SaveChangesAsync();
                id = accounts.First().Id;
            }

            // Act
            using (var context = CreateDbContext("Delete_Account"))
            {
                var repository = new AccountRepository(context);
                await repository.Delete(id.Value);
                result = await repository.SaveChangesAsync();
            }


            // Assert
            result.Should().Be(1);
            // Simulate access from another context to verifiy that correct data was saved to database
            using (var context = CreateDbContext("Delete_Account"))
            {
                (await context.Set<Account>().FirstOrDefaultAsync(x => x.Id == id)).Should().BeNull();
                (await context.Set<Account>().ToListAsync()).Should().NotBeEmpty();
            }
        }
    }
}
