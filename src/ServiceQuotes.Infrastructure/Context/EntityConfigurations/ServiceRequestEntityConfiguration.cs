using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceQuotes.Domain.Entities;

namespace ServiceQuotes.Infrastructure.Context.EntityConfigurations
{
    public class ServiceRequestEntityConfiguration : IEntityTypeConfiguration<ServiceRequest>
    {
        public void Configure(EntityTypeBuilder<ServiceRequest> builder)
        {
            builder.HasKey(sr => sr.Id);

            builder.Property(sr => sr.Id).HasDefaultValueSql("uuid_generate_v4()").IsRequired();

            builder.Property(sr => sr.Title).HasMaxLength(50).IsRequired();

            builder.Property(sr => sr.Description).IsRequired();

            builder
                .HasOne(sr => sr.Customer)
                .WithMany(c => c.ServiceRequests)
                .HasForeignKey(sr => sr.CustomerId);

            builder
                .HasOne(sr => sr.Address)
                .WithMany(c => c.ServiceRequests)
                .HasForeignKey(sr => sr.AddressId);

            builder
                .HasOne(sr => sr.CustomerAddress)
                .WithMany(ca => ca.ServiceRequests)
                .HasForeignKey(sr => new { sr.CustomerId, sr.AddressId });
        }
    }
}
