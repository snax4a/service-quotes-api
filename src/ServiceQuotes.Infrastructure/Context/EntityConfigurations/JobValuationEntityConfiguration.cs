using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceQuotes.Domain.Entities;

namespace ServiceQuotes.Infrastructure.Context.EntityConfigurations
{
    public class JobValuationEntityConfiguration : IEntityTypeConfiguration<JobValuation>
    {
        public void Configure(EntityTypeBuilder<JobValuation> builder)
        {
            builder.HasKey(jv => jv.Id);

            builder.Property(jv => jv.Id).ValueGeneratedOnAdd().IsRequired();

            builder.Property(jv => jv.WorkType).HasMaxLength(100).IsRequired();

            builder.Property(jv => jv.HourlyRate).HasPrecision(7, 2).IsRequired();

            builder.Property(jv => jv.LaborHours).HasColumnType("time").IsRequired();
        }
    }
}
