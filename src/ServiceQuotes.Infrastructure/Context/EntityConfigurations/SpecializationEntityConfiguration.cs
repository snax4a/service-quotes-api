using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceQuotes.Domain.Entities;

namespace ServiceQuotes.Infrastructure.Context.EntityConfigurations
{
    internal class SpecializationEntityConfiguration : IEntityTypeConfiguration<Specialization>
    {
        public void Configure(EntityTypeBuilder<Specialization> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id).HasDefaultValueSql("uuid_generate_v4()").IsRequired();

            builder.Property(s => s.Name).HasMaxLength(40).IsRequired();

            builder.HasIndex(s => s.Name).IsUnique();
        }
    }
}
