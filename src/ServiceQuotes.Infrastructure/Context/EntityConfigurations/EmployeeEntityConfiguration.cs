using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceQuotes.Domain.Entities;
using System;

namespace ServiceQuotes.Infrastructure.Context.EntityConfigurations
{
    public class EmployeeEntityConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(employee => employee.Id);

            builder.Property(employee => employee.Id).ValueGeneratedOnAdd().IsRequired();

            builder.Property(employee => employee.FirstName).HasMaxLength(30).IsRequired();

            builder.Property(employee => employee.LastName).HasMaxLength(30).IsRequired();

            builder.HasOne(employee => employee.Account).WithOne().HasForeignKey<Employee>("AccountId").IsRequired();

            builder.HasIndex("AccountId").IsUnique();

            builder.HasData(
                new Employee
                {
                    Id = new Guid("5e02401f-bf8c-4e2f-b4a8-a7e27cd3678d"),
                    AccountId = new Guid("7542b6b8-638c-44c9-806b-0040667c32a9"),
                    FirstName = "Szymon",
                    LastName = "Sus"
                }
            );
        }
    }
}
