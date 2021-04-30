using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceQuotes.Domain.Entities;
using ServiceQuotes.Domain.Entities.Enums;
using System;

namespace ServiceQuotes.Infrastructure.Context.EntityConfigurations
{
    public class AccountEntityConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasData(
                new Account
                {
                    Id = new Guid("7542b6b8-638c-44c9-806b-0040667c32a9"),
                    Email = "manager@service-quotes.com",
                    PasswordHash = "$2a$11$RtlK2XAwpq2cY0EOnZlVJOpcM7BKnTbUNy50tZ14D57Og8iZcP5pi", //manager12345
                    Role = Role.Manager,
                    Created = new DateTime(2021, 3, 1, 22, 46, 58, 919, DateTimeKind.Utc).AddTicks(9540)
                }
            );
        }
    }
}
