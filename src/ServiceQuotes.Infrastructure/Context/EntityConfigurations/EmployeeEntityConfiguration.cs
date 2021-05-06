using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceQuotes.Domain.Entities;

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
        }
    }
}
