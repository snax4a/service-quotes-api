using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceQuotes.Domain.Entities;

namespace ServiceQuotes.Infrastructure.Context.EntityConfigurations
{
    public class MaterialEntityConfiguration : IEntityTypeConfiguration<Material>
    {
        public void Configure(EntityTypeBuilder<Material> builder)
        {
            builder.HasKey(material => material.Id);

            builder.Property(material => material.Id).HasDefaultValueSql("uuid_generate_v4()").IsRequired();

            builder.Property(material => material.Description).HasMaxLength(100).IsRequired();

            builder.Property(material => material.Quantity).IsRequired();

            builder.Property(material => material.UnitPrice).HasPrecision(7, 2).IsRequired();

            builder
                .HasOne(material => material.ServiceRequest)
                .WithMany(sr => sr.Materials)
                .HasForeignKey(material => material.ServiceRequestId);
        }
    }
}
