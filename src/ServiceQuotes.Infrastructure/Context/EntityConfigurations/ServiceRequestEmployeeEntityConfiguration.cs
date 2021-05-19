using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceQuotes.Domain.Entities;

namespace ServiceQuotes.Infrastructure.Context.EntityConfigurations
{
    public class ServiceRequestEmployeeEntityConfiguration : IEntityTypeConfiguration<ServiceRequestEmployee>
    {
        public void Configure(EntityTypeBuilder<ServiceRequestEmployee> builder)
        {
            builder.HasKey(sre => new { sre.ServiceRequestId, sre.EmployeeId });

            builder
                .HasOne(sre => sre.ServiceRequest)
                .WithMany(sr => sr.ServiceRequestEmployees)
                .HasForeignKey(sre => sre.ServiceRequestId);

            builder
                .HasOne(sre => sre.Employee)
                .WithMany(e => e.ServiceRequestEmployees)
                .HasForeignKey(sre => sre.EmployeeId);
        }
    }
}
