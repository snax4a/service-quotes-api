using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceQuotes.Domain.Entities;

namespace ServiceQuotes.Infrastructure.Context.EntityConfigurations
{
    public class ServiceRequestJobValuationEntityConfiguration : IEntityTypeConfiguration<ServiceRequestJobValuation>
    {
        public void Configure(EntityTypeBuilder<ServiceRequestJobValuation> builder)
        {
            builder.HasKey(entity => new { entity.EmployeeId, entity.JobValuationId, entity.ServiceRequestId });

            builder.Property(entity => entity.Date).IsRequired();

            builder
                .HasOne(entity => entity.Employee)
                .WithMany(employee => employee.ServiceRequestJobValuations)
                .HasForeignKey(entity => entity.EmployeeId);

            builder
                .HasOne(entity => entity.JobValuation)
                .WithMany(jobValuation => jobValuation.ServiceRequestJobValuations)
                .HasForeignKey(entity => entity.JobValuationId);

            builder
                .HasOne(entity => entity.ServiceRequest)
                .WithMany(serviceRequest => serviceRequest.ServiceRequestJobValuations)
                .HasForeignKey(entity => entity.ServiceRequestId);
        }
    }
}
